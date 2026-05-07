using Microsoft.EntityFrameworkCore;
using SirTebz_Tech.Data;
using SirTebz_Tech.Models.Entities;
using SirTebz_Tech.Repositories.Interfaces;

namespace SirTebz_Tech.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    public ProductRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Product>> GetAllAsync()
        => await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

    public async Task<Product?> GetByIdAsync(int id)
        => await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

    public async Task<Product?> GetByIdWithSpecsAsync(int id)
        => await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Specifications.OrderBy(s => s.DisplayOrder))
            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

    public async Task<(IEnumerable<Product> products, int totalCount)> GetFilteredAsync(
        int? categoryId, int? brandId, decimal? minPrice, decimal? maxPrice,
        string? ram, string? storage, string? cpu,
        string? sortBy, string? searchTerm, int page, int pageSize)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Specifications)
            .Where(p => p.IsActive)
            .AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (brandId.HasValue)
            query = query.Where(p => p.BrandId == brandId.Value);

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);

        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(p =>
                p.Name.Contains(searchTerm) ||
                p.Description.Contains(searchTerm) ||
                p.Brand.Name.Contains(searchTerm) ||
                p.Category.Name.Contains(searchTerm));

        if (!string.IsNullOrWhiteSpace(ram))
            query = query.Where(p => p.Specifications.Any(s =>
                s.SpecKey == "RAM" && s.SpecValue.Contains(ram)));

        if (!string.IsNullOrWhiteSpace(storage))
            query = query.Where(p => p.Specifications.Any(s =>
                s.SpecKey == "Storage" && s.SpecValue.Contains(storage)));

        if (!string.IsNullOrWhiteSpace(cpu))
            query = query.Where(p => p.Specifications.Any(s =>
                s.SpecKey == "CPU" && s.SpecValue.Contains(cpu)));

        query = sortBy switch
        {
            "price_asc" => query.OrderBy(p => p.Price),
            "price_desc" => query.OrderByDescending(p => p.Price),
            "name_asc" => query.OrderBy(p => p.Name),
            "newest" => query.OrderByDescending(p => p.CreatedAt),
            _ => query.OrderByDescending(p => p.IsFeatured).ThenByDescending(p => p.CreatedAt)
        };

        var totalCount = await query.CountAsync();
        var products = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (products, totalCount);
    }

    public async Task<IEnumerable<Product>> GetFeaturedAsync(int count = 8)
        => await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Where(p => p.IsActive && p.IsFeatured)
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync();

    public async Task<IEnumerable<Product>> GetRelatedAsync(int productId, int categoryId, int count = 4)
        => await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Where(p => p.IsActive && p.CategoryId == categoryId && p.Id != productId)
            .Take(count)
            .ToListAsync();

    public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<int> ids)
        => await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Specifications.OrderBy(s => s.DisplayOrder))
            .Where(p => ids.Contains(p.Id) && p.IsActive)
            .ToListAsync();

    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            product.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
        => await _context.Products.AnyAsync(p => p.Id == id && p.IsActive);

    public async Task<List<string>> GetSpecValuesByKeyAsync(string specKey)
        => await _context.ProductSpecifications
            .Where(s => s.SpecKey == specKey)
            .Select(s => s.SpecValue)
            .Distinct()
            .OrderBy(v => v)
            .ToListAsync();
}