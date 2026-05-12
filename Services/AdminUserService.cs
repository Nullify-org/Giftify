using Giftify.Interfaces.Services;      // IAdminUserService
using Giftify.Models;
using Giftify.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Giftify.Services;

public class AdminUserService : IAdminUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole>    _roleManager;

    public AdminUserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole>    roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IEnumerable<AdminUserListVM>> GetAllUsersAsync()
    {
        var users  = await _userManager.Users.ToListAsync();
        var result = new List<AdminUserListVM>();

        foreach (var u in users)
        {
            var roles = await _userManager.GetRolesAsync(u);
            result.Add(new AdminUserListVM
            {
                Id        = u.Id,
                FullName  = u.FullName,
                UserName  = u.UserName ?? "",
                Email     = u.Email    ?? "",
                Roles     = roles.ToList(),
                CreatedAt = u.CreatedAt
            });
        }

        return result;
    }

    public async Task<IdentityResult> CreateUserAsync(AdminUserCreateVM vm)
    {
        var user = new ApplicationUser
        {
            FullName       = vm.FullName,
            UserName       = vm.UserName,
            Email          = vm.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, vm.Password);

        if (result.Succeeded && !string.IsNullOrEmpty(vm.Role))
            await _userManager.AddToRoleAsync(user, vm.Role);

        return result;
    }

    public async Task<IdentityResult> DeleteUserAsync(string userId, string currentUserName)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return IdentityResult.Failed(
                new IdentityError { Description = "User not found." });

        if (user.UserName == currentUserName)
            return IdentityResult.Failed(
                new IdentityError { Description = "You cannot delete your own account." });

        return await _userManager.DeleteAsync(user);
    }

    public async Task<IEnumerable<string>> GetAvailableRolesAsync()
        => await _roleManager.Roles.Select(r => r.Name!).ToListAsync();
}
