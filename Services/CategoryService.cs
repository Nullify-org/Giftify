using Giftify.Interfaces.Services;
using Giftify.Interfaces;
using Giftify.Models;
using Giftify.ViewModels.Categories;
using Giftify.ViewModels.Occasions;
using Giftify.ViewModels.Products;
namespace Giftify.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryVM>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            var categoriesVM = categories.Select(c => new CategoryVM
            {
                Id = c.Id,
                Name = c.Name,
                //Description = c.Description,
                //IsActive = c.IsActive
            });
            return categoriesVM;
        }
        public async Task<CategoryDetailsVM> GetCategoryDetailsAsync(int categoryId)
        {
            string[] includes = { nameof(Category.Products) };
            var category = await _unitOfWork.Categories.FindAsync(c => c.Id == categoryId, includes);
            if (category == null)
                return null;
            var categoryDetailsVM = new CategoryDetailsVM
            {
                Id = category.Id,
                Name = category.Name,
                ProductCount = category.Products?.Count() ?? 0
            };
            return categoryDetailsVM;

        }
        public async Task AddCategoryAsync(CategoryCreateVM categoryCreateVM)
        {
            var category = new Category
            {
                Name = categoryCreateVM.Name
            };
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.Save();
        }

        public async Task UpdateCategoryAsync(int categoryId, CategoryEditVM categoryEditVM)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);
            if (category == null)
                throw new Exception("Category not found");
            category.Name = categoryEditVM.Name;
            _unitOfWork.Categories.Update(category);
            await _unitOfWork.Save();
        }
    }
}

