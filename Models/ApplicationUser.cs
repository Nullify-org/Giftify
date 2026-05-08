//using Microsoft.AspNetCore.Identity;

namespace Giftify.Models;
// ashraf modification 
/*public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? ProfileImage { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
*/


using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

[NotMapped]
public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }

    public DateTime? DateOfBirth { get; set; }
    public string? ProfileImage { get; set; }
    public string? City { get; set; }

    public string? Street { get; set; }

    public string? Building { get; set; }

    public string? AddressNotes { get; set; }

    public DateTime CreatedAt { get; set; }
        = DateTime.Now;

    // Navigation Properties
    public ICollection<Order> Orders { get; set; }
        = new List<Order>();
    // as it has one to one relation with cart
    public Cart? Cart { get; set; }
}
