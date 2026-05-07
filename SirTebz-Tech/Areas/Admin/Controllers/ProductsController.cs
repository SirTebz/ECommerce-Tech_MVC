using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SirTebz_Tech.Models.ViewModels;
using SirTebz_Tech.Services.Interfaces;

namespace SirTebz_Tech.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly IWebHostEnvironment _env;

    public ProductsController(IProductService productService, IWebHostEnvironment env)
    {
        _productService = productService;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetProductsAsync(
            null, null, null, null, null, null, null, null, null, 1, 100);
        return View(products.Products);
    }

    public async Task<IActionResult> Create()
    {
        var vm = await _productService.GetProductForEditAsync();
        return View(vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCreateEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var reloaded = await _productService.GetProductForEditAsync();
            model.Categories = reloaded.Categories;
            model.Brands = reloaded.Brands;
            return View(model);
        }
        var id = await _productService.CreateProductAsync(model, _env.WebRootPath);
        TempData["Success"] = "Product created successfully!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var vm = await _productService.GetProductForEditAsync(id);
        if (vm == null) return NotFound();
        return View(vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductCreateEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var reloaded = await _productService.GetProductForEditAsync(model.Id);
            model.Categories = reloaded!.Categories;
            model.Brands = reloaded.Brands;
            return View(model);
        }
        await _productService.UpdateProductAsync(model, _env.WebRootPath);
        TempData["Success"] = "Product updated successfully!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteProductAsync(id);
        TempData["Success"] = "Product deleted.";
        return RedirectToAction(nameof(Index));
    }
}