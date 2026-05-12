using System.ComponentModel.DataAnnotations;

namespace Giftify.ViewModels.Admin;

public class AdminProductEditVM
{
    public int Id { get; set; }

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

    public bool IsActive { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    public int CategoryId { get; set; }

    // shown in the form so admin can see the current image
    public string? ExistingImageUrl { get; set; }

    // optional — if admin picks a new image
    public IFormFile? ImageFile { get; set; }

    // Occasions (optional, multi-select)
    public List<int> SelectedOccasionIds { get; set; } = new();
}