using SirTebzTech.Models.Entities;

namespace SirTebzTech.Models.Entities;

public class Brand
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<Product> Products { get; set; } = new List<Product>();
}