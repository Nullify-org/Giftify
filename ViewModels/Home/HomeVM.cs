using Giftify.ViewModels.Occasions;
using Giftify.ViewModels.Products;
using Giftify.ViewModels.Categories;

namespace Giftify.ViewModels.Home
{
    public class HomeVM
    {
        public IEnumerable<OccasionVM> Occasions { get; set; } = new List<OccasionVM>();
        public IEnumerable<CategoryVM> Categories { get; set; } = new List<CategoryVM>();
        public IEnumerable<ProductListItemVM> FeaturedProducts { get; set; } = new List<ProductListItemVM>();
    }
}