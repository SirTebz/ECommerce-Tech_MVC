using SirTebz_Tech.Models.Entities;
using SirTebz_Tech.Models.ViewModels;

namespace SirTebz_Tech.Services.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<int> GetTotalCountAsync();
    Task<decimal> GetTotalRevenueAsync();
    Task<Order?> GetOrderAsync(int id);
    Task<Order> PlaceOrderAsync(string userId);
}