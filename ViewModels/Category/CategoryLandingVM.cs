namespace Giftify.ViewModels.Categories;

public class CategoryLandingVM
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string? CategoryDescription { get; set; }
    public int TotalProducts { get; set; }
    public List<CategoryProductItemVM> Products { get; set; } = new();
    public List<RelatedCategoryVM> RelatedCategories { get; set; } = new();
}

/// <summary>Product row shown inside a category landing page.</summary>
public class CategoryProductItemVM
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public bool IsInStock { get; set; }
    public int Stock { get; set; }
}

/// <summary>Small sibling-category pill shown in the category sidebar.</summary>
public class RelatedCategoryVM
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ProductCount { get; set; }
}
