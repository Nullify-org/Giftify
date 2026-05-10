namespace Giftify.ViewModels.Home;

public class HomeIndexVM
{
    public HeroVM Hero { get; set; } = new();
    public List<HomeCategoryCardVM> FeaturedCategories { get; set; } = new();
    public List<HomeProductCardVM> FeaturedProducts { get; set; } = new();
    public List<HomeOccasionBadgeVM> Occasions { get; set; } = new();
}

public class HomeProductCardVM
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsInStock { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}

public class HomeCategoryCardVM
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string IconClass { get; set; } = "bi-gift";
    public int ProductCount { get; set; }
    public string GradientFrom { get; set; } = "#0F3D2E";
    public string GradientTo { get; set; } = "#2a7a58";
}

public class HomeOccasionBadgeVM
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Emoji { get; set; } = "🎁";
    public string IconClass { get; set; } = "bi-gift";
    public int GiftCount { get; set; }
    public string GradientFrom { get; set; } = "#0F3D2E";
    public string GradientTo { get; set; } = "#2a7a58";
    public string TextColor { get; set; } = "#fff";
}