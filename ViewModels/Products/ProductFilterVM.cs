namespace Giftify.ViewModels.Products;

public class ProductFilterVM
{
    public string? SearchTerm { get; set; }
    public int? DeartmentId { get; set; }
    public int? OccasionId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}
