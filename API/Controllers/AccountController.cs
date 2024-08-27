using API.DTOs;
using API.Extensions;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly UserManager<User> _userManager;
    private readonly TokenService _tokenService;
    private readonly StoreContext _context;

    public AccountController(UserManager<User> userManager, TokenService tokenService, StoreContext storeContext)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _context = storeContext;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
    {
        var user = await _userManager.FindByNameAsync(loginDTO.Username);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDTO.Password))
        {
            return Unauthorized();
        }

        var userCart = await RetrieveCart(loginDTO.Username);
        var anonymousCart = await RetrieveCart(Request.Cookies["buyerId"]);

        if (anonymousCart != null)
        {
            if (userCart != null) _context.Carts.Remove(userCart);
            anonymousCart.BuyerId = user.UserName;
            Response.Cookies.Delete("buyerId");
            await _context.SaveChangesAsync();
        }

        return new UserDTO
        {
            Email = user.Email,
            Token = await _tokenService.GenerateToken(user),
            Cart = anonymousCart != null ? anonymousCart.CartToDTO() : userCart?.CartToDTO()
        };
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDTO registerDTO)
    {
        var user = new User { UserName = registerDTO.Username, Email = registerDTO.Email };
        var result = await _userManager.CreateAsync(user, registerDTO.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem();
        }

        await _userManager.AddToRoleAsync(user, "Member");
        return StatusCode(201);
    }

    [Authorize]
    [HttpGet("currentUser")]
    public async Task<ActionResult<UserDTO>> GetCurrentUser()
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        var userCart = await RetrieveCart(User.Identity.Name);

        return new UserDTO
        {
            Email = user.Email,
            Token = await _tokenService.GenerateToken(user),
            Cart = userCart?.CartToDTO()
        };
    }

    [Authorize]
    [HttpGet("savedAddress")]
    public async Task<ActionResult<UserAddress>> GetSavedAddres()
    {
        return await _userManager.Users
            .Where(x => x.UserName == User.Identity.Name)
            .Select(user => user.Address)
            .FirstOrDefaultAsync();
    }


    private async Task<Cart> RetrieveCart(string buyerId)
    {
        if (string.IsNullOrEmpty(buyerId))
        {
            Response.Cookies.Delete("buyerId");
        }
        return await _context.Carts
            .Include(i => i.Items)
            .ThenInclude(p => p.Product)
            .FirstOrDefaultAsync(x => x.BuyerId == buyerId);
    }
}