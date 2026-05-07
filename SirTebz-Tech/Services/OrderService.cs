using SirTebzTech.Models.Entities;
using SirTebzTech.Models.ViewModels;
using SirTebzTech.Repositories.Interfaces;
using SirTebzTech.Services.Interfaces;

namespace SirTebzTech.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepo;
    private readonly ICartService _cartService;
    private readonly ICartRepository _cartRepo;

    public OrderService(IOrderRepository orderRepo, ICartService cartService, ICartRepository cartRepo)
    {
        _orderRepo = orderRepo;
        _cartService = cartService;
        _cartRepo = cartRepo;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync() => await _orderRepo.GetAllAsync();
    public async Task<int> GetTotalCountAsync() => await _orderRepo.GetTotalCountAsync();
    public async Task<decimal> GetTotalRevenueAsync() => await _orderRepo.GetTotalRevenueAsync();
    public async Task<Order?> GetOrderAsync(int id) => await _orderRepo.GetByIdAsync(id);

    public async Task<Order> PlaceOrderAsync(string userId)
    {
        var cartVm = await _cartService.GetCartAsync(userId);
        var order = new Order
        {
            UserId = userId,
            TotalAmount = cartVm.Total,
            OrderItems = cartVm.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.Price
            }).ToList()
        };
        var created = await _orderRepo.CreateAsync(order);
        await _cartService.ClearCartAsync(userId);
        return created;
    }
}