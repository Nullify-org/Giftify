namespace Giftify.ViewModels.Products;

public class ProductFilterVM
{
    public string? SearchTerm { get; set; }
<<<<<<< HEAD
    public int? CategoryId { get; set; }
    public List<int>? OccasionIds { get; set; } = new List<int>();
=======
    public int? DeartmentId { get; set; }
    public int? OccasionId { get; set; }
>>>>>>> 1e4876017e0475229b7cb5fd3cb178f81b5991d5
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}
