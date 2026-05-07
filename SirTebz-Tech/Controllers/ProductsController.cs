using Microsoft.AspNetCore.Mvc;
using SirTebz_Tech.Services.Interfaces;

namespace SirTebz_Tech.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index(
        int? categoryId, int? brandId,
        decimal? minPrice, decimal? maxPrice,
        string? ram, string? storage, string? cpu,
        string? sortBy, string? searchTerm, int page = 1)
    {
        var model = await _productService.GetProductsAsync(
            categoryId, brandId, minPrice, maxPrice,
            ram, storage, cpu, sortBy, searchTerm, page, 12);
        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var model = await _productService.GetProductDetailAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }
}