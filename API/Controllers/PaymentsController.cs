using System.Reflection.Metadata.Ecma335;
using API.DTOs;
using API.Extensions;
using API.Models;
using API.Models.OrderAggregate;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace API.Controllers;

public class PaymentsController : BaseApiController
{
    private readonly PaymentService _paymentService;
    private readonly StoreContext _context;
    private readonly IConfiguration _config;

    public PaymentsController(PaymentService paymentService, StoreContext context, IConfiguration config)
    {
        _paymentService = paymentService;
        _context = context;
        _config = config;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CartDTO>> CreateOrUpdatePaymentIntent()
    {
        var cart = await _context.Carts
            .RetrieveCartWithItems(User.Identity.Name)
            .FirstOrDefaultAsync();

        if (cart == null) return NotFound();
        var intent = await _paymentService.CreateOrUpdatePyamentIntent(cart);
        if (intent == null) return BadRequest(new ProblemDetails { Title = "Probelm creating payment intent." });

        cart.PaymentIntentId = cart.PaymentIntentId ?? intent.Id;
        cart.ClientSecret = cart.ClientSecret ?? intent.ClientSecret;

        _context.Update(cart);

        var result = await _context.SaveChangesAsync() > 0;
        if (!result) return BadRequest(new ProblemDetails { Title = "Problem updating cart with intent " });

        return cart.CartToDTO();
    }

    [HttpPost("webhook")]
    public async Task<ActionResult> StripeWebhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"],
            _config["StripeSettings:WhSecret"]);
        var charge = (Charge)stripeEvent.Data.Object;
        var order = await _context.Orders.FirstOrDefaultAsync(x => x.PaymentIntentId == charge.PaymentIntentId);

        if (charge.Status == "succeeded") order.OrderStatus = OrderStatus.PaymentReceived;
        await _context.SaveChangesAsync();

        return new EmptyResult();
    }
}
