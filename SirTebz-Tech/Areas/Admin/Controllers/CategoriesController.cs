using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SirTebz_Tech.Models.ViewModels;
using SirTebz_Tech.Services.Interfaces;

namespace SirTebz_Tech.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;
    public CategoriesController(ICategoryService categoryService) => _categoryService = categoryService;

    public async Task<IActionResult> Index()
        => View(await _categoryService.GetAllAsync());

    public async Task<IActionResult> Create() => View(await _categoryService.GetForEditAsync());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryCreateEditViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        await _categoryService.CreateAsync(model);
        TempData["Success"] = "Category created!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var vm = await _categoryService.GetForEditAsync(id);
        if (vm == null) return NotFound();
        return View(vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CategoryCreateEditViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        await _categoryService.UpdateAsync(model);
        TempData["Success"] = "Category updated!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _categoryService.DeleteAsync(id);
        TempData["Success"] = "Category deleted.";
        return RedirectToAction(nameof(Index));
    }
}