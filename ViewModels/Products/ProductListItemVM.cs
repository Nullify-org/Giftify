using Giftify.ViewModels.Categories;
using Giftify.ViewModels.Occasions;

namespace Giftify.ViewModels.Products;

public class ProductListItemVM
{
    public int Id { get; set; }
    public string ImageUrl { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public bool IsInStock { get; set; }
}
