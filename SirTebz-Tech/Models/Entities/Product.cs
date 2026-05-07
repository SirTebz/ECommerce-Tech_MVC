using SirTebz_Tech.Models.Entities;

namespace SirTebz_Tech.Models.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? OriginalPrice { get; set; }
    public string? ImageUrl { get; set; }
    public int Stock { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public int CategoryId { get; set; }
    public int BrandId { get; set; }

    // Navigation
    public Category Category { get; set; } = null!;
    public Brand Brand { get; set; } = null!;
    public ICollection<ProductSpecification> Specifications { get; set; } = new List<ProductSpecification>();
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    // Computed
    public bool IsOnSale => OriginalPrice.HasValue && OriginalPrice > Price;
    public int DiscountPercent => IsOnSale ? (int)(((OriginalPrice!.Value - Price) / OriginalPrice.Value) * 100) : 0;
}