using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SirTebz_Tech.Models.Entities;
using SirTebz_Tech.Models.ViewModels;
using SirTebz_Tech.Services.Interfaces;

namespace SirTebz_Tech.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DashboardController : Controller
{
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;
    private readonly ICategoryService _categoryService;
    private readonly UserManager<ApplicationUser> _userManager;

    public DashboardController(
        IProductService productService,
        IOrderService orderService,
        ICategoryService categoryService,
        UserManager<ApplicationUser> userManager)
    {
        _productService = productService;
        _orderService = orderService;
        _categoryService = categoryService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var featuredProducts = await _productService.GetFeaturedProductsAsync(5);
        var orders = await _orderService.GetAllOrdersAsync();
        var categories = await _categoryService.GetAllAsync();
        var users = _userManager.Users.ToList();

        var vm = new AdminDashboardViewModel
        {
            TotalProducts = featuredProducts.Count,
            TotalOrders = await _orderService.GetTotalCountAsync(),
            TotalUsers = users.Count,
            TotalCategories = categories.Count(),
            TotalRevenue = await _orderService.GetTotalRevenueAsync(),
            RecentProducts = featuredProducts,
            RecentOrders = orders.Take(5).Select(o => new OrderSummaryViewModel
            {
                Id = o.Id,
                UserEmail = o.User?.Email ?? "Unknown",
                TotalAmount = o.TotalAmount,
                Status = o.Status.ToString(),
                CreatedAt = o.CreatedAt
            }).ToList()
        };
        return View(vm);
    }
}