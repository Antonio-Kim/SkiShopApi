using API.DTOs;
using API.Extensions;
using API.Models;
using API.Models.OrderAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class OrdersController : BaseApiController
{
    private readonly StoreContext _context;
    public OrdersController(StoreContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderDTO>>> GetOrders()
    {
        return await _context.Orders
            .ProjectOrderToOrderDTO()
            .Where(x => x.BuyerId == User.Identity.Name)
            .ToListAsync();
    }

    [HttpGet("{id}", Name = "GetOrder")]
    public async Task<ActionResult<OrderDTO>> GetOrder(int id)
    {
        return await _context.Orders
            .ProjectOrderToOrderDTO()
            .Where(x => x.BuyerId == User.Identity.Name && x.Id == id)
            .FirstOrDefaultAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(CreateOrderDTO orderDTO)
    {
        var cart = await _context.Carts
            .RetrieveCartWithItems(User.Identity.Name)
            .FirstOrDefaultAsync();

        if (cart == null) return BadRequest(new ProblemDetails { Title = "Could not locate cart" });
        var items = new List<OrderItem>();
        foreach (var item in cart.Items)
        {
            var productItem = await _context.Products.FindAsync(item.ProductId);
            var itemOrdered = new ProductItemOrdered
            {
                ProductId = productItem.Id,
                Name = productItem.Name,
                PictureUrl = productItem.PictureUrl
            };
            var orderItem = new OrderItem
            {
                ItemOrdered = itemOrdered,
                Price = productItem.Price,
                Quantity = item.Quantity
            };
            items.Add(orderItem);
            productItem.QuantityInStock -= item.Quantity;
        }

        var subtotal = items.Sum(item => item.Price * item.Quantity);
        var deliveryFee = subtotal > 10000 ? 0 : 500;

        var order = new Order
        {
            OrderItems = items,
            BuyerId = User.Identity.Name,
            ShippingAddress = orderDTO.ShippingAddress,
            Subtotal = subtotal,
            DeliveryFee = deliveryFee,
        };

        _context.Orders.Add(order);
        _context.Carts.Remove(cart);

        if (orderDTO.SaveAddress)
        {
            var user = await _context.Users
                .Include(a => a.Address)
                .FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            var address = new UserAddress
            {
                FullName = orderDTO.ShippingAddress.FullName,
                Address1 = orderDTO.ShippingAddress.Address1,
                Address2 = orderDTO.ShippingAddress.Address2,
                City = orderDTO.ShippingAddress.City,
                State = orderDTO.ShippingAddress.State,
                Zip = orderDTO.ShippingAddress.Zip,
                Country = orderDTO.ShippingAddress.Country,
            };
            user.Address = address;
        }

        var result = await _context.SaveChangesAsync() > 0;

        if (result) return CreatedAtRoute("GetOrder", new { id = order.Id }, order.Id);

        return BadRequest("Problem creating order");
    }
}