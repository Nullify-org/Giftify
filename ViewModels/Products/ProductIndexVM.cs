using Giftify.ViewModels.Categories;
using Giftify.ViewModels.Occasions;

namespace Giftify.ViewModels.Products;

public class ProductIndexVM
{
    public IEnumerable<ProductListItemVM> Products { get; set; } = new List<ProductListItemVM>();
    public IEnumerable<CategoryVM> Categories { get; set; } = new List<CategoryVM>();
    public IEnumerable<OccasionVM> Occasions { get; set; } = new List<OccasionVM>();
    public ProductFilterVM CurrentFilters { get; set; } = new ProductFilterVM();

}
