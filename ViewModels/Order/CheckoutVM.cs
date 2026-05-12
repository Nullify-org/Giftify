using System.ComponentModel.DataAnnotations;
using Giftify.Enums;

namespace Giftify.ViewModels.Order;

public class CheckoutVM
{
    // ── Address ─────────────────────────────────────────────────────────────

    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(150)]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Invalid phone number.")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Street address is required.")]
    [StringLength(300)]
    public string Street { get; set; }

    [Required(ErrorMessage = "City is required.")]
    [StringLength(100)]
    public string City { get; set; }

    [StringLength(100)]
    public string? State { get; set; }

    [StringLength(20)]
    public string? PostalCode { get; set; }

    // ── Gift ─────────────────────────────────────────────────────────────────

    [StringLength(300)]
    public string? GiftMessage { get; set; }

    // ── Payment ──────────────────────────────────────────────────────────────

    [Required]
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.CashOnDelivery;

    // ── Order Summary (read-only, populated from cart) ───────────────────────

    public List<CheckoutItemVM> Items { get; set; } = new();
    public decimal Subtotal  => Items.Sum(i => i.LineTotal);
    public decimal Shipping  { get; set; } = 0;
    public decimal Total     => Subtotal + Shipping;
}

public class CheckoutItemVM
{
    public int    ProductId  { get; set; }
    public string Name       { get; set; }
    public string ImageUrl   { get; set; }
    public int    Quantity   { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal => UnitPrice * Quantity;
}
