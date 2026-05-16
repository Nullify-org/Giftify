using Giftify.ViewModels.Categories;
using Giftify.ViewModels.Occasions;
using System.ComponentModel.DataAnnotations;

namespace Giftify.ViewModels.Products;

public class EditProductVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Product name is required")]
    public string Name { get; set; }
    public string? Description { get; set; }

    [Range(1, 100000)]
    public decimal Price { get; set; }

    [Range(0, 1000)]
    public int Stock { get; set; }

    [Display(Name = "Category"), Required(ErrorMessage = "Please select a category")]
    public int CategoryId { get; set; }

    //[Required(ErrorMessage = "Please upload at least one image for the product.")]
    public ICollection<IFormFile>? Images { get; set; } = new List<IFormFile>();

    [Required(ErrorMessage = "You must select at least one occasion for the product.")]
    [MinLength(1, ErrorMessage = "You must select at least one occasion for the product.")]
    public List<int> SelectedOccasionIds { get; set; } = new List<int>();
    public ICollection<CategoryVM>? Categories { get; set; }
    public ICollection<OccasionVM>? Occasions { get; set; }
    public ICollection<ProductImageVM> ExistingImages { get; set; } = new List<ProductImageVM>();
}
