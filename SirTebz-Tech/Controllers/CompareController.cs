using Microsoft.AspNetCore.Mvc;
using SirTebz_Tech.Services.Interfaces;

namespace SirTebz_Tech.Controllers;

public class CompareController : Controller
{
    private readonly IProductService _productService;

    public CompareController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index(string? ids)
    {
        if (string.IsNullOrWhiteSpace(ids))
            return View(null);

        var productIds = ids.Split(',')
            .Select(id => int.TryParse(id.Trim(), out var parsed) ? parsed : 0)
            .Where(id => id > 0)
            .Distinct()
            .Take(4)
            .ToList();

        if (!productIds.Any())
            return View(null);

        var model = await _productService.GetCompareViewModelAsync(productIds);
        return View(model);
    }
}