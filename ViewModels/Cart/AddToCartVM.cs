using System.ComponentModel.DataAnnotations;

namespace Giftify.ViewModels.Cart;

public class AddToCartVM
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100.")]
    public int Quantity { get; set; } = 1;
}
