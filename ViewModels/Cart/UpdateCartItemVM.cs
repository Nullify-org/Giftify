using System.ComponentModel.DataAnnotations;

namespace Giftify.ViewModels.Cart;

public class UpdateCartItemVM
{
    [Required]
    public int CartItemId { get; set; }

    [Required]
    [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100.")]
    public int NewQuantity { get; set; }
}
