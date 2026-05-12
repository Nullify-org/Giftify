using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Giftify.Validations;

public class RequiredListAttribute : ValidationAttribute, IClientModelValidator
{
    public RequiredListAttribute()
    {
        ErrorMessage = "At least one item must be selected.";
    }

    // ── Server-side ───────────────────────────────────────────────────────────
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is System.Collections.ICollection collection && collection.Count > 0)
            return ValidationResult.Success;

        return new ValidationResult(
            ErrorMessage,
            new[] { validationContext.MemberName! }
        );
    }

    // ── Client-side (jQuery Unobtrusive Validation) ───────────────────────────
    public void AddValidation(ClientModelValidationContext context)
    {
        context.Attributes.TryAdd("data-val",              "true");
        context.Attributes.TryAdd("data-val-requiredlist", ErrorMessage!);
    }
}
