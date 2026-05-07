using Microsoft.AspNetCore.Mvc;
using SirTebz_Tech.Services.Interfaces;

namespace SirTebz_Tech.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public HomeController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        var featured = await _productService.GetFeaturedProductsAsync(8);
        var categories = await _categoryService.GetAllAsync();
        ViewBag.Categories = categories;
        return View(featured);
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View();
}