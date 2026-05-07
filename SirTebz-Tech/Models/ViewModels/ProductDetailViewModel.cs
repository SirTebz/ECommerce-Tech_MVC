namespace SirTebz_Tech.Models.ViewModels;

public class ProductDetailViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? OriginalPrice { get; set; }
    public string? ImageUrl { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string BrandName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public int BrandId { get; set; }
    public int Stock { get; set; }
    public bool IsOnSale { get; set; }
    public int DiscountPercent { get; set; }
    public List<SpecificationViewModel> Specifications { get; set; } = new();
    public List<ProductCardViewModel> RelatedProducts { get; set; } = new();
}

public class SpecificationViewModel
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}