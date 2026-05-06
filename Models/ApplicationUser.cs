using Microsoft.AspNetCore.Identity;

namespace Giftify.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? ProfileImage { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
