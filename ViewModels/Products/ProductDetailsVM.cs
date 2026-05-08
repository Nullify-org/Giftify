using Giftify.Models;
using Giftify.ViewModels.Categories;
using Giftify.ViewModels.Occasions;

namespace Giftify.ViewModels.Products;

public class ProductDetailsVM
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public CategoryVM Category { get; set; }
    public List<string> ImageUrls { get; set; }

    public decimal Price { get; set; }
    public int StockQuantity { get; set; }

    public List<OccasionVM> Occasions { get; set; }

}
