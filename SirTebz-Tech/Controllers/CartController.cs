using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SirTebz_Tech.Services.Interfaces;
using System.Security.Claims;

namespace SirTebz_Tech.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public async Task<IActionResult> Index()
    {
        var cart = await _cartService.GetCartAsync(UserId);
        return View(cart);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
    {
        await _cartService.AddToCartAsync(UserId, productId, quantity);
        var count = await _cartService.GetCartCountAsync(UserId);

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return Json(new { success = true, cartCount = count, message = "Item added to cart!" });
        }
        TempData["Success"] = "Item added to cart!";
        return RedirectToAction("Index", "Products");
    }

    [HttpPost]
    public async Task<IActionResult> Remove(int cartItemId)
    {
        await _cartService.RemoveFromCartAsync(UserId, cartItemId);
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return Json(new { success = true });
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
    {
        await _cartService.UpdateQuantityAsync(UserId, cartItemId, quantity);
        var cart = await _cartService.GetCartAsync(UserId);
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return Json(new { success = true, total = cart.Total.ToString("C"), itemCount = cart.ItemCount });
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> GetCartCount()
    {
        var count = await _cartService.GetCartCountAsync(UserId);
        return Json(new { count });
    }

    [HttpPost]
    public async Task<IActionResult> Checkout()
    {
        var cart = await _cartService.GetCartAsync(UserId);
        if (!cart.Items.Any())
        {
            TempData["Error"] = "Your cart is empty.";
            return RedirectToAction(nameof(Index));
        }
        // For now, just show success (full checkout in future phase)
        await _cartService.ClearCartAsync(UserId);
        TempData["Success"] = "Order placed successfully! Thank you for your purchase.";
        return RedirectToAction("Index", "Home");
    }
}