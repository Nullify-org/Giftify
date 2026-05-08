namespace Giftify.ViewModels.Categories;

public class CategoryVM
{
    public string Name { get; set; }

    public string Description { get; set; }

    public bool IsActive { get; set; }
    public int Id { get; internal set; }
}
