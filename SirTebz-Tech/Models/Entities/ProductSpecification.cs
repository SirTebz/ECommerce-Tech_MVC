namespace SirTebz_Tech.Models.Entities;

public class ProductSpecification
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string SpecKey { get; set; } = string.Empty;
    public string SpecValue { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }

    // Navigation
    public Product Product { get; set; } = null!;
}