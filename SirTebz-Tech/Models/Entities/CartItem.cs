namespace SirTebzTech.Models.Entities;

public class CartItem
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Cart Cart { get; set; } = null!;
    public Product Product { get; set; } = null!;

    // Computed
    public decimal Subtotal => Product?.Price * Quantity ?? 0;
}