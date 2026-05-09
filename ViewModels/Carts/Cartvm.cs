namespace Giftify.ViewModels.Cart;

public class CartVM
{
    public int CartId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public List<CartItemVM> Items { get; set; } = new();

    public int TotalItems => Items.Sum(i => i.Quantity);
    public decimal Subtotal => Items.Sum(i => i.LineTotal);
}
