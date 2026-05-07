using SirTebzTech.Models.ViewModels;

namespace SirTebzTech.Services.Interfaces;

public interface ICartService
{
    Task<CartViewModel> GetCartAsync(string userId);
    Task<int> GetCartCountAsync(string userId);
    Task AddToCartAsync(string userId, int productId, int quantity = 1);
    Task RemoveFromCartAsync(string userId, int cartItemId);
    Task UpdateQuantityAsync(string userId, int cartItemId, int quantity);
    Task ClearCartAsync(string userId);
}