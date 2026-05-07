using SirTebzTech.Models.Entities;
using SirTebzTech.Models.ViewModels;

namespace SirTebzTech.Services.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<int> GetTotalCountAsync();
    Task<decimal> GetTotalRevenueAsync();
    Task<Order?> GetOrderAsync(int id);
    Task<Order> PlaceOrderAsync(string userId);
}