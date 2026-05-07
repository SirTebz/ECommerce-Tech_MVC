namespace SirTebz_Tech.Models.ViewModels;

public class ProductListViewModel
{
    public List<ProductCardViewModel> Products { get; set; } = new();
    public ProductFilterViewModel Filters { get; set; } = new();
    public PaginationViewModel Pagination { get; set; } = new();
    public string? SearchTerm { get; set; }
    public int TotalCount { get; set; }
}

public class ProductFilterViewModel
{
    public int? CategoryId { get; set; }
    public int? BrandId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? Ram { get; set; }
    public string? Storage { get; set; }
    public string? Cpu { get; set; }
    public string? SortBy { get; set; }
    public List<CategoryFilterOption> Categories { get; set; } = new();
    public List<BrandFilterOption> Brands { get; set; } = new();
    public List<string> RamOptions { get; set; } = new();
    public List<string> StorageOptions { get; set; } = new();
}

public class CategoryFilterOption
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ProductCount { get; set; }
}

public class BrandFilterOption
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ProductCount { get; set; }
}

public class PaginationViewModel
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
}