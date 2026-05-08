namespace Giftify.ViewModels;
using System.ComponentModel.DataAnnotations;

    public class RegisterVM
    {
     [Required]
     [StringLength(100)]
    public string FullName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? ProfileImage { get; set; }

    public string? City { get; set; }

    public string? Street { get; set; }

    public string? Building { get; set; }

    public string? AddressNotes { get; set; }






    }

