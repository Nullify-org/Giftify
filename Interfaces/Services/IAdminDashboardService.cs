using Giftify.ViewModels.Admin;

namespace Giftify.Interfaces.Services;

public interface IAdminDashboardService
{
    Task<AdminDashboardVM> GetDashboardDataAsync();
}
