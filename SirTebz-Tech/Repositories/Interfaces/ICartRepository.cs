using SirTebz_Tech.Models.Entities;

namespace SirTebz_Tech.Repositories.Interfaces;

public interface ICartRepository
{
    Task<Cart?> GetCartByUserIdAsync(string userId);
    Task<Cart> CreateCartAsync(string userId);
    Task<CartItem?> GetCartItemAsync(int cartId, int productId);
    Task AddCartItemAsync(CartItem item);
    Task UpdateCartItemAsync(CartItem item);
    Task RemoveCartItemAsync(int cartItemId);
    Task ClearCartAsync(int cartId);
    Task SaveChangesAsync();
}