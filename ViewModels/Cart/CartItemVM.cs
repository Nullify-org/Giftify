namespace Giftify.ViewModels.Cart;

public class CartItemVM
{
    public int CartItemId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ImageUrl { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public int Stock { get; set; }                  
    public decimal LineTotal => UnitPrice * Quantity;
}