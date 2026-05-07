using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SirTebz_Tech.Models.ViewModels;
using SirTebz_Tech.Services.Interfaces;

namespace SirTebz_Tech.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class BrandsController : Controller
{
    private readonly IBrandService _brandService;
    public BrandsController(IBrandService brandService) => _brandService = brandService;

    public async Task<IActionResult> Index() => View(await _brandService.GetAllAsync());

    public async Task<IActionResult> Create() => View(await _brandService.GetForEditAsync());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BrandCreateEditViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        await _brandService.CreateAsync(model);
        TempData["Success"] = "Brand created!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var vm = await _brandService.GetForEditAsync(id);
        if (vm == null) return NotFound();
        return View(vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(BrandCreateEditViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        await _brandService.UpdateAsync(model);
        TempData["Success"] = "Brand updated!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _brandService.DeleteAsync(id);
        TempData["Success"] = "Brand deleted.";
        return RedirectToAction(nameof(Index));
    }
}