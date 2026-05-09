using Giftify.ViewModels.Categories;
using Giftify.ViewModels.Occasions;

namespace Giftify.ViewModels.Products;

public class ProductListItemVM
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    // FIX: carry all image URLs for the hero-collage slider
    public List<string> AllImageUrls { get; set; } = new List<string>();
    public bool IsInStock { get; set; }
    public decimal Price { get; set; }
    // FIX: needed for controller-level filtering
    public int CategoryId { get; set; }
    public List<int> OccasionIds { get; set; } = new List<int>();
    public List<CategoryVM> Categories { get; set; }
    public List<OccasionVM> Occasions { get; set; }
}
