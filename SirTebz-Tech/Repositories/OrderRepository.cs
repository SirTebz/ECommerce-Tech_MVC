using Microsoft.EntityFrameworkCore;
using SirTebz_Tech.Data;
using SirTebz_Tech.Models.Entities;
using SirTebz_Tech.Repositories.Interfaces;

namespace SirTebz_Tech.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;
    public OrderRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Order>> GetAllAsync()
        => await _context.Orders.Include(o => o.User).Include(o => o.OrderItems)
            .OrderByDescending(o => o.CreatedAt).ToListAsync();

    public async Task<IEnumerable<Order>> GetByUserIdAsync(string userId)
        => await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
            .Where(o => o.UserId == userId).OrderByDescending(o => o.CreatedAt).ToListAsync();

    public async Task<Order?> GetByIdAsync(int id)
        => await _context.Orders.Include(o => o.User).Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product).FirstOrDefaultAsync(o => o.Id == id);

    public async Task<Order> CreateAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetTotalCountAsync()
        => await _context.Orders.CountAsync();

    public async Task<decimal> GetTotalRevenueAsync()
        => await _context.Orders.Where(o => o.Status != OrderStatus.Cancelled)
            .SumAsync(o => o.TotalAmount);
}