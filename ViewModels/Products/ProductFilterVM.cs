namespace Giftify.ViewModels.Products;

public class ProductFilterVM
{
    public string? SearchTerm { get; set; }
    public int? CategoryId { get; set; }
    public List<int>? OccasionIds { get; set; } = new List<int>();
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}
