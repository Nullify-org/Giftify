namespace Giftify.Models;

public class Cart
{
    public int Id { get; set; }
    public string ApplicationUserId { get; set; }
    public ApplicationUser User { get; set; }
    public DateTime CreatedAt { get; set; }
         = DateTime.Now;

    // Navigation property for cart items
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
