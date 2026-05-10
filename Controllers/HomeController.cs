using System.Diagnostics;
using Giftify.Interfaces;
using Giftify.Models;
using Giftify.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;

namespace Giftify.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    private static readonly string[] OccasionIcons =
    {
        "bi-cake2", "bi-ring", "bi-heart-fill", "bi-flower1",
        "bi-person-check", "bi-mortarboard", "bi-stars",
        "bi-moon-stars", "bi-sun", "bi-balloon-heart",
    };

    private static readonly string[] OccasionEmojis =
    {
        "🎂", "💍", "❤️", "🌹", "👔", "🎓", "🎆", "🌙", "🎉", "🥂"
    };

    private static readonly (string From, string To, string Text)[] OccasionGradients =
    {
        ("#ffecd2", "#fcb69f", "#1C1C1A"),
        ("#f8cdda", "#1d2b64", "#fff"),
        ("#ff9a9e", "#fecfef", "#1C1C1A"),
        ("#f953c6", "#b91d73", "#fff"),
        ("#0f3443", "#34e89e", "#fff"),
        ("#0F3D2E", "#2a7a58", "#fff"),
        ("#667eea", "#764ba2", "#fff"),
        ("#1d2b64", "#f8cdda", "#fff"),
        ("#E8A020", "#f5c55a", "#1C1C1A"),
        ("#43e97b", "#38f9d7", "#1C1C1A"),
    };

    private static readonly string[] CategoryIcons =
    {
        "bi-headphones", "bi-bag-heart", "bi-book", "bi-house-heart",
        "bi-bicycle", "bi-stars", "bi-controller", "bi-cup-hot",
        "bi-gem", "bi-cpu",
    };

    private static readonly (string From, string To)[] CategoryGradients =
    {
        ("#fccb90", "#d57eeb"),
        ("#f093fb", "#f5576c"),
        ("#4facfe", "#00f2fe"),
        ("#E8A020", "#f5c55a"),
        ("#43e97b", "#38f9d7"),
        ("#a18cd1", "#fbc2eb"),
        ("#fa709a", "#fee140"),
        ("#f953c6", "#b91d73"),
        ("#667eea", "#764ba2"),
        ("#0F3D2E", "#2a7a58"),
    };

    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index()
    {
        // ── Products ──────────────────────────────────────────
        var allProducts = await _unitOfWork.Products.GetAllAsync(p => p.Images, p => p.Category);
        var featuredProducts = allProducts
            .Where(p => p.IsActive)
            .Take(8)
            .Select(p => new HomeProductCardVM
            {
                Id           = p.Id,
                Name         = p.Name,
                Price        = p.Price,
                ImageUrl     = p.Images?.FirstOrDefault(i => i.IsPrimary)?.ImageUrl
                               ?? p.Images?.FirstOrDefault()?.ImageUrl
                               ?? "/images/default-product.png",
                IsInStock    = p.Stock > 0,
                CategoryName = p.Category?.Name ?? string.Empty,
            }).ToList();

        // ── Categories — names from DB, icons assigned by index ──
        var allCategories = await _unitOfWork.Categories.GetAllAsync(c => c.Products);
        var featuredCategories = allCategories
            .Where(c => c.IsActive)
            .Take(8)
            .Select((c, i) => new HomeCategoryCardVM
            {
                Id           = c.Id,
                Name         = c.Name,
                Description  = c.Description,
                ProductCount = c.Products?.Count ?? 0,
                IconClass    = CategoryIcons[i % CategoryIcons.Length],
                GradientFrom = CategoryGradients[i % CategoryGradients.Length].From,
                GradientTo   = CategoryGradients[i % CategoryGradients.Length].To,
            }).ToList();

        // ── Occasions — names from DB, icons assigned by index ──
        var allOccasions = await _unitOfWork.Occasions.GetAllAsync();
        var occasions = allOccasions
            .Where(o => o.IsActive)
            .Take(10)
            .Select((o, i) => new HomeOccasionBadgeVM
            {
                Id           = o.Id,
                Name         = o.Name,
                Emoji        = OccasionEmojis[i % OccasionEmojis.Length],
                IconClass    = OccasionIcons[i % OccasionIcons.Length],
                GiftCount    = o.OccasionProducts?.Count ?? 0,
                GradientFrom = OccasionGradients[i % OccasionGradients.Length].From,
                GradientTo   = OccasionGradients[i % OccasionGradients.Length].To,
                TextColor    = OccasionGradients[i % OccasionGradients.Length].Text,
            }).ToList();

        // ── Hero ──────────────────────────────────────────────
        var hero = new HeroVM
        {
            HeroImageUrl = "https://images.unsplash.com/photo-1549465220-1a8b9238cd48?w=800&q=80"
        };

        var vm = new HomeIndexVM
        {
            Hero               = hero,
            FeaturedProducts   = featuredProducts,
            FeaturedCategories = featuredCategories,
            Occasions          = occasions,
        };

        return View(vm);
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
