namespace Giftify.ViewModels.Home;

public class HeroVM
{
    public string Headline { get; set; } = "Find the Perfect Gift for Every Occasion";
    public string SubHeadline { get; set; } = "Handpicked gifts for birthdays, weddings, holidays, and more — delivered with love.";
    public string PrimaryCtaText { get; set; } = "Shop Now";
    public string PrimaryCtaUrl { get; set; } = "/Products";
    public string SecondaryCtaText { get; set; } = "Explore Occasions";
    public string SecondaryCtaUrl { get; set; } = "/Products";
    public string HeroImageUrl { get; set; } = "/images/hero-bg.jpg";
    public List<string> TrustBadges { get; set; } = new()
    {
        "Free shipping over 500 EGP",
        "Easy 30-day returns",
        "Gift wrapping available"
    };
}
