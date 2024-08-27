using API.DTOs;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class CartExtensions
{
    public static CartDTO CartToDTO(this Cart cart)
    {
        return new CartDTO
        {
            Id = cart.Id,
            BuyerId = cart.BuyerId,
            Items = cart.Items.Select(item => new CartItemDTO
            {
                ProductId = item.ProductId,
                Name = item.Product.Name,
                Price = item.Product.Price,
                PictureUrl = item.Product.PictureUrl,
                Type = item.Product.Type,
                Brand = item.Product.Brand,
                Quantity = item.Quantity,
            }).ToList()
        };
    }

    public static IQueryable<Cart> RetrieveCartWithItems(this IQueryable<Cart> query, string buyerId)
    {
        return query.Include(i => i.Items)
            .ThenInclude(p => p.Product)
            .Where(b => b.BuyerId == buyerId);
    }
}