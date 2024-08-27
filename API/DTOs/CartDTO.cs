namespace API.DTOs;

public class CartDTO
{
    public int Id { get; set; }
    public string BuyerId { get; set; }
    public List<CartItemDTO> Items { get; set; }
    public string PaymentIntentId { get; set; }
    public string ClientSecret { get; set; }
}