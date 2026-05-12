using System.ComponentModel.DataAnnotations;

namespace Giftify.ViewModels.Admin;

// ── User list ────────────────────────────────────────────
public class AdminUserListVM
{
    public string Id        { get; set; } = "";
    public string FullName  { get; set; } = "";
    public string UserName  { get; set; } = "";
    public string Email     { get; set; } = "";
    public List<string> Roles { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

// ── Create user ──────────────────────────────────────────
public class AdminUserCreateVM
{
    [Required]
    public string FullName { get; set; } = "";

    [Required]
    public string UserName { get; set; } = "";

    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required, MinLength(6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    public string? Role { get; set; }
}
