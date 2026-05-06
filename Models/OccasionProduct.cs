namespace Giftify.Models;

public class OccasionProduct
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int OccasionId { get; set; }
    public Product Product { get; set; }
    public Occasion Occasion { get; set; }
}
