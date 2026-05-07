using Microsoft.AspNetCore.Mvc.Rendering;
using SirTebz_Tech.Models.Entities;
using SirTebz_Tech.Models.ViewModels;
using SirTebz_Tech.Repositories.Interfaces;
using SirTebz_Tech.Services.Interfaces;

namespace SirTebz_Tech.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly IBrandRepository _brandRepo;

    public ProductService(
        IProductRepository productRepo,
        ICategoryRepository categoryRepo,
        IBrandRepository brandRepo)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
        _brandRepo = brandRepo;
    }

    public async Task<ProductListViewModel> GetProductsAsync(
        int? categoryId, int? brandId, decimal? minPrice, decimal? maxPrice,
        string? ram, string? storage, string? cpu,
        string? sortBy, string? searchTerm, int page = 1, int pageSize = 12)
    {
        var (products, totalCount) = await _productRepo.GetFilteredAsync(
            categoryId, brandId, minPrice, maxPrice, ram, storage, cpu,
            sortBy, searchTerm, page, pageSize);

        var categories = await _categoryRepo.GetAllAsync();
        var brands = await _brandRepo.GetAllAsync();
        var ramOptions = await _productRepo.GetSpecValuesByKeyAsync("RAM");
        var storageOptions = await _productRepo.GetSpecValuesByKeyAsync("Storage");

        return new ProductListViewModel
        {
            Products = products.Select(MapToCard).ToList(),
            TotalCount = totalCount,
            SearchTerm = searchTerm,
            Filters = new ProductFilterViewModel
            {
                CategoryId = categoryId,
                BrandId = brandId,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Ram = ram,
                Storage = storage,
                Cpu = cpu,
                SortBy = sortBy,
                Categories = categories.Select(c => new CategoryFilterOption
                {
                    Id = c.Id,
                    Name = c.Name,
                    ProductCount = c.Products.Count
                }).ToList(),
                Brands = brands.Select(b => new BrandFilterOption
                {
                    Id = b.Id,
                    Name = b.Name,
                    ProductCount = b.Products.Count
                }).ToList(),
                RamOptions = ramOptions,
                StorageOptions = storageOptions
            },
            Pagination = new PaginationViewModel
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            }
        };
    }

    public async Task<ProductDetailViewModel?> GetProductDetailAsync(int id)
    {
        var product = await _productRepo.GetByIdWithSpecsAsync(id);
        if (product == null) return null;

        var related = await _productRepo.GetRelatedAsync(id, product.CategoryId, 4);

        return new ProductDetailViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            OriginalPrice = product.OriginalPrice,
            ImageUrl = product.ImageUrl,
            CategoryName = product.Category.Name,
            BrandName = product.Brand.Name,
            CategoryId = product.CategoryId,
            BrandId = product.BrandId,
            Stock = product.Stock,
            IsOnSale = product.IsOnSale,
            DiscountPercent = product.DiscountPercent,
            Specifications = product.Specifications.Select(s => new SpecificationViewModel
            {
                Key = s.SpecKey,
                Value = s.SpecValue,
                DisplayOrder = s.DisplayOrder
            }).ToList(),
            RelatedProducts = related.Select(MapToCard).ToList()
        };
    }

    public async Task<List<ProductCardViewModel>> GetFeaturedProductsAsync(int count = 8)
    {
        var products = await _productRepo.GetFeaturedAsync(count);
        return products.Select(MapToCard).ToList();
    }

    public async Task<CompareViewModel> GetCompareViewModelAsync(IEnumerable<int> productIds)
    {
        var products = await _productRepo.GetByIdsAsync(productIds);
        var compareItems = products.Select(p => new ProductCompareItem
        {
            Id = p.Id,
            Name = p.Name,
            ImageUrl = p.ImageUrl,
            Price = p.Price,
            BrandName = p.Brand.Name,
            CategoryName = p.Category.Name,
            Specifications = p.Specifications
                .OrderBy(s => s.DisplayOrder)
                .ToDictionary(s => s.SpecKey, s => s.SpecValue)
        }).ToList();

        var allSpecKeys = compareItems
            .SelectMany(p => p.Specifications.Keys)
            .Distinct()
            .OrderBy(k => k)
            .ToList();

        return new CompareViewModel
        {
            Products = compareItems,
            AllSpecKeys = allSpecKeys
        };
    }

    public async Task<ProductCreateEditViewModel> GetProductForEditAsync(int? id = null)
    {
        var categories = await _categoryRepo.GetAllAsync();
        var brands = await _brandRepo.GetAllAsync();

        var vm = new ProductCreateEditViewModel
        {
            IsActive = true,
            Categories = categories.Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToList(),
            Brands = brands.Select(b => new SelectListItem(b.Name, b.Id.ToString())).ToList()
        };

        if (id.HasValue)
        {
            var product = await _productRepo.GetByIdWithSpecsAsync(id.Value);
            if (product != null)
            {
                vm.Id = product.Id;
                vm.Name = product.Name;
                vm.Description = product.Description;
                vm.Price = product.Price;
                vm.OriginalPrice = product.OriginalPrice;
                vm.CategoryId = product.CategoryId;
                vm.BrandId = product.BrandId;
                vm.Stock = product.Stock;
                vm.IsFeatured = product.IsFeatured;
                vm.IsActive = product.IsActive;
                vm.ImageUrl = product.ImageUrl;
                vm.Specifications = product.Specifications
                    .OrderBy(s => s.DisplayOrder)
                    .Select(s => new SpecEditViewModel { Key = s.SpecKey, Value = s.SpecValue, DisplayOrder = s.DisplayOrder })
                    .ToList();
            }
        }
        return vm;
    }

    public async Task<int> CreateProductAsync(ProductCreateEditViewModel model, string wwwRootPath)
    {
        var imageUrl = await SaveImageAsync(model.ImageFile, wwwRootPath);
        var product = new Product
        {
            Name = model.Name,
            Description = model.Description,
            Price = model.Price,
            OriginalPrice = model.OriginalPrice,
            CategoryId = model.CategoryId,
            BrandId = model.BrandId,
            Stock = model.Stock,
            IsFeatured = model.IsFeatured,
            IsActive = model.IsActive,
            ImageUrl = imageUrl ?? model.ImageUrl,
            Specifications = model.Specifications
                .Where(s => !string.IsNullOrWhiteSpace(s.Key))
                .Select((s, i) => new ProductSpecification
                {
                    SpecKey = s.Key,
                    SpecValue = s.Value,
                    DisplayOrder = i + 1
                }).ToList()
        };
        var created = await _productRepo.CreateAsync(product);
        return created.Id;
    }

    public async Task UpdateProductAsync(ProductCreateEditViewModel model, string wwwRootPath)
    {
        var product = await _productRepo.GetByIdWithSpecsAsync(model.Id);
        if (product == null) return;

        if (model.ImageFile != null)
            product.ImageUrl = await SaveImageAsync(model.ImageFile, wwwRootPath);

        product.Name = model.Name;
        product.Description = model.Description;
        product.Price = model.Price;
        product.OriginalPrice = model.OriginalPrice;
        product.CategoryId = model.CategoryId;
        product.BrandId = model.BrandId;
        product.Stock = model.Stock;
        product.IsFeatured = model.IsFeatured;
        product.IsActive = model.IsActive;
        product.Specifications = model.Specifications
            .Where(s => !string.IsNullOrWhiteSpace(s.Key))
            .Select((s, i) => new ProductSpecification
            {
                ProductId = product.Id,
                SpecKey = s.Key,
                SpecValue = s.Value,
                DisplayOrder = i + 1
            }).ToList();

        await _productRepo.UpdateAsync(product);
    }

    public async Task DeleteProductAsync(int id) => await _productRepo.DeleteAsync(id);

    private async Task<string?> SaveImageAsync(IFormFile? imageFile, string wwwRootPath)
    {
        if (imageFile == null || imageFile.Length == 0) return null;
        var uploadsFolder = Path.Combine(wwwRootPath, "images", "products");
        Directory.CreateDirectory(uploadsFolder);
        var uniqueFileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
        await using var stream = new FileStream(filePath, FileMode.Create);
        await imageFile.CopyToAsync(stream);
        return $"/images/products/{uniqueFileName}";
    }

    private static ProductCardViewModel MapToCard(Product p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description.Length > 100 ? p.Description[..100] + "..." : p.Description,
        Price = p.Price,
        OriginalPrice = p.OriginalPrice,
        ImageUrl = p.ImageUrl,
        CategoryName = p.Category.Name,
        BrandName = p.Brand.Name,
        Stock = p.Stock,
        IsOnSale = p.IsOnSale,
        DiscountPercent = p.DiscountPercent,
        IsFeatured = p.IsFeatured
    };
}