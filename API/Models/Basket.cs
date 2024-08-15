namespace API.Models;

public class Basket
{
	public int Id { get; set; }
	public string BuyerId { get; set; }
	public List<BasketItem> Items { get; set; } = new List<BasketItem>();

	public void AddItem(Product product, int quantity)
	{
		if (Items.All(item => item.ProductId != product.Id))
		{
			Items.Add(new BasketItem { Product = product, Quantity = quantity });
		}
		var existingItem = Items.FirstOrDefault(item => item.ProductId == product.Id);
		if (existingItem != null) existingItem.Quantity += quantity;
	}

	public void RemoveItem(int productId, int quantity)
	{
		var item = Items.FirstOrDefault(item => item.ProductId == productId);
		if (item == null) return;
		if (item.Quantity > quantity) item.Quantity -= quantity;
		else if (item.Quantity == quantity) Items.Remove(item);
	}
}