using Giftify.Data;
using Giftify.Interfaces.Repositories;
using Giftify.Models;
using Giftify.ViewModels.Categories;
using Giftify.ViewModels.Occasions;
using Giftify.ViewModels.Products;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Giftify.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> SearchAsync(ProductFilterVM model)
    {
        if (model == null)
            return await _context.Products.Include(p => p.Images).ToListAsync();

        IQueryable<Product> query = _context.Products.Include(p => p.Images);

        if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            query = query.Where(p => p.Name.Contains(model.SearchTerm));

        if (model.CategoryId.HasValue)
            query = query.Where(p => p.CategoryId == model.CategoryId.Value);

        if (model.OccasionIds != null && model.OccasionIds.Any())
            query = query.Where(p => p.OccasionProducts.Any(op => model.OccasionIds.Contains(op.OccasionId)));

        if (model.MinPrice.HasValue)
            query = query.Where(p => p.Price >= model.MinPrice.Value);

        if (model.MaxPrice.HasValue)
            query = query.Where(p => p.Price <= model.MaxPrice.Value);

        return await query.ToListAsync();
    }
}
