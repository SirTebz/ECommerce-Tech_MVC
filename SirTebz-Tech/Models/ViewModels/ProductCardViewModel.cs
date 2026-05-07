namespace SirTebzTech.Models.ViewModels;

public class ProductCardViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? OriginalPrice { get; set; }
    public string? ImageUrl { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string BrandName { get; set; } = string.Empty;
    public int Stock { get; set; }
    public bool IsOnSale { get; set; }
    public int DiscountPercent { get; set; }
    public bool IsFeatured { get; set; }
}