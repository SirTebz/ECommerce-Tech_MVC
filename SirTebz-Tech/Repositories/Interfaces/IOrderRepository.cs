using SirTebzTech.Models.Entities;

namespace SirTebzTech.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<IEnumerable<Order>> GetByUserIdAsync(string userId);
    Task<Order?> GetByIdAsync(int id);
    Task<Order> CreateAsync(Order order);
    Task UpdateAsync(Order order);
    Task<int> GetTotalCountAsync();
    Task<decimal> GetTotalRevenueAsync();
}