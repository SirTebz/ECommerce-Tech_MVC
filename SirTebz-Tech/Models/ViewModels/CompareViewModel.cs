namespace SirTebz_Tech.Models.ViewModels;

public class CompareViewModel
{
    public List<ProductCompareItem> Products { get; set; } = new();
    public List<string> AllSpecKeys { get; set; } = new();
}

public class ProductCompareItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public Dictionary<string, string> Specifications { get; set; } = new();
}