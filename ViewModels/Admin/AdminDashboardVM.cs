namespace Giftify.ViewModels.Admin;

public class AdminDashboardVM
{
    public int     TotalProducts    { get; set; }
    public int     TotalCategories  { get; set; }
    public int     TotalOccasions   { get; set; }
    public int     TotalOrders      { get; set; }
    public int     TotalUsers       { get; set; }
    public decimal TotalRevenue     { get; set; }

    public int PendingOrders    { get; set; }
    public int ProcessingOrders { get; set; }
    public int ShippedOrders    { get; set; }
    public int DeliveredOrders  { get; set; }

    public IEnumerable<Giftify.Models.Order> RecentOrders { get; set; } = new List<Giftify.Models.Order>();
}
