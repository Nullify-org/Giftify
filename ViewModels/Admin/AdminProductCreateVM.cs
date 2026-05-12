using System.ComponentModel.DataAnnotations;
using Giftify.Validations;

namespace Giftify.ViewModels.Admin;

public class AdminProductCreateVM
{
    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(200)]
    public string Name { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, 999999, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Stock is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
    public int Stock { get; set; }

    public bool IsActive { get; set; } = true;

    [Required(ErrorMessage = "Category is required.")]
    public int CategoryId { get; set; }

    public IFormFile? ImageFile { get; set; }

    [RequiredList(ErrorMessage = "At least one occasion is required.")]
    public List<int> SelectedOccasionIds { get; set; } = new();
}
