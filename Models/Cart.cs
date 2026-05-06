namespace Giftify.Models;

public class Cart
{
    public int Id { get; set; }
    public int ApplicationUserId { get; set; }
    public ApplicationUser User { get; set; }
}
