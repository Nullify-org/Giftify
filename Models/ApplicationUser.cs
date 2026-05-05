using Microsoft.AspNetCore.Identity;

namespace Giftify.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
    public string Address { get; set; }
    public DateTime DateOfBirth { get; set; }
    public IFormFile ProfileImage { get; set; }
}
