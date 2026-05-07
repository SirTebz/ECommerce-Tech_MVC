using SirTebz_Tech.Models.Entities;

namespace SirTebz_Tech.Repositories.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product?> GetByIdWithSpecsAsync(int id);
    Task<(IEnumerable<Product> products, int totalCount)> GetFilteredAsync(
        int? categoryId, int? brandId, decimal? minPrice, decimal? maxPrice,
        string? ram, string? storage, string? cpu,
        string? sortBy, string? searchTerm, int page, int pageSize);
    Task<IEnumerable<Product>> GetFeaturedAsync(int count = 8);
    Task<IEnumerable<Product>> GetRelatedAsync(int productId, int categoryId, int count = 4);
    Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<int> ids);
    Task<Product> CreateAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<List<string>> GetSpecValuesByKeyAsync(string specKey);
}