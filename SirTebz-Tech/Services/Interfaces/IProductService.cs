using SirTebz_Tech.Models.ViewModels;

namespace SirTebz_Tech.Services.Interfaces;

public interface IProductService
{
    Task<ProductListViewModel> GetProductsAsync(
        int? categoryId, int? brandId, decimal? minPrice, decimal? maxPrice,
        string? ram, string? storage, string? cpu,
        string? sortBy, string? searchTerm, int page = 1, int pageSize = 12);
    Task<ProductDetailViewModel?> GetProductDetailAsync(int id);
    Task<List<ProductCardViewModel>> GetFeaturedProductsAsync(int count = 8);
    Task<CompareViewModel> GetCompareViewModelAsync(IEnumerable<int> productIds);
    Task<ProductCreateEditViewModel> GetProductForEditAsync(int? id = null);
    Task<int> CreateProductAsync(ProductCreateEditViewModel model, string wwwRootPath);
    Task UpdateProductAsync(ProductCreateEditViewModel model, string wwwRootPath);
    Task DeleteProductAsync(int id);
}