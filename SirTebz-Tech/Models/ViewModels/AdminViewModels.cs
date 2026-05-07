using System.ComponentModel.DataAnnotations;

namespace SirTebz_Tech.Models.ViewModels;

public class AdminDashboardViewModel
{
    public int TotalProducts { get; set; }
    public int TotalOrders { get; set; }
    public int TotalUsers { get; set; }
    public int TotalCategories { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<ProductCardViewModel> RecentProducts { get; set; } = new();
    public List<OrderSummaryViewModel> RecentOrders { get; set; } = new();
}

public class OrderSummaryViewModel
{
    public int Id { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class ProductCreateEditViewModel
{
    public int Id { get; set; }

    [Required, StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required, Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    public decimal? OriginalPrice { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public int BrandId { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; } = true;
    public string? ImageUrl { get; set; }
    public IFormFile? ImageFile { get; set; }

    public List<SpecEditViewModel> Specifications { get; set; } = new();
    public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> Categories { get; set; } = new();
    public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> Brands { get; set; } = new();
}

public class SpecEditViewModel
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}

public class CategoryCreateEditViewModel
{
    public int Id { get; set; }
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IconClass { get; set; }
    public bool IsActive { get; set; } = true;
}

public class BrandCreateEditViewModel
{
    public int Id { get; set; }
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public bool IsActive { get; set; } = true;
}