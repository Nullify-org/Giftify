using Giftify.ViewModels.Categories;

namespace Giftify.Interfaces.Services

{
    public interface ICategoryService
    {
         Task<IEnumerable<CategoryVM>> GetAllCategoriesAsync();
         Task<CategoryDetailsVM> GetCategoryDetailsAsync(int categoryId);
         Task AddCategoryAsync(CategoryCreateVM categoryCreateVM);
         Task UpdateCategoryAsync(int categoryId, CategoryEditVM categoryEditVM);
    }
}
