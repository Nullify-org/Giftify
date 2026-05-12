using Giftify.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;

namespace Giftify.Interfaces.Services;

public interface IAdminUserService
{
    Task<IEnumerable<AdminUserListVM>> GetAllUsersAsync();
    Task<IdentityResult>              CreateUserAsync(AdminUserCreateVM vm);
    Task<IdentityResult>              DeleteUserAsync(string userId, string currentUserName);
    Task<IEnumerable<string>>         GetAvailableRolesAsync();
}
