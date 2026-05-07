using SirTebz_Tech.Models.Entities;

namespace SirTebz_Tech.Models.Entities;

public class Cart
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ApplicationUser User { get; set; } = null!;
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    // Computed
    public decimal Total => CartItems.Sum(ci => ci.Subtotal);
    public int ItemCount => CartItems.Sum(ci => ci.Quantity);
}