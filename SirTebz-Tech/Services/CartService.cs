using SirTebz_Tech.Models.Entities;
using SirTebz_Tech.Models.ViewModels;
using SirTebz_Tech.Repositories.Interfaces;
using SirTebz_Tech.Services.Interfaces;

namespace SirTebz_Tech.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepo;
    private readonly IProductRepository _productRepo;

    public CartService(ICartRepository cartRepo, IProductRepository productRepo)
    {
        _cartRepo = cartRepo;
        _productRepo = productRepo;
    }

    private async Task<Cart> GetOrCreateCartAsync(string userId)
    {
        var cart = await _cartRepo.GetCartByUserIdAsync(userId);
        if (cart == null)
            cart = await _cartRepo.CreateCartAsync(userId);
        return cart;
    }

    public async Task<CartViewModel> GetCartAsync(string userId)
    {
        var cart = await _cartRepo.GetCartByUserIdAsync(userId);
        if (cart == null) return new CartViewModel();

        var items = cart.CartItems.Select(ci => new CartItemViewModel
        {
            CartItemId = ci.Id,
            ProductId = ci.ProductId,
            ProductName = ci.Product.Name,
            ImageUrl = ci.Product.ImageUrl,
            Price = ci.Product.Price,
            Quantity = ci.Quantity,
            Subtotal = ci.Product.Price * ci.Quantity,
            MaxStock = ci.Product.Stock
        }).ToList();

        return new CartViewModel
        {
            CartId = cart.Id,
            Items = items,
            Total = items.Sum(i => i.Subtotal),
            ItemCount = items.Sum(i => i.Quantity)
        };
    }

    public async Task<int> GetCartCountAsync(string userId)
    {
        var cart = await _cartRepo.GetCartByUserIdAsync(userId);
        return cart?.CartItems.Sum(ci => ci.Quantity) ?? 0;
    }

    public async Task AddToCartAsync(string userId, int productId, int quantity = 1)
    {
        var cart = await GetOrCreateCartAsync(userId);
        var existingItem = await _cartRepo.GetCartItemAsync(cart.Id, productId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
            await _cartRepo.UpdateCartItemAsync(existingItem);
        }
        else
        {
            await _cartRepo.AddCartItemAsync(new CartItem
            {
                CartId = cart.Id,
                ProductId = productId,
                Quantity = quantity
            });
        }
    }

    public async Task RemoveFromCartAsync(string userId, int cartItemId)
        => await _cartRepo.RemoveCartItemAsync(cartItemId);

    public async Task UpdateQuantityAsync(string userId, int cartItemId, int quantity)
    {
        var cart = await _cartRepo.GetCartByUserIdAsync(userId);
        if (cart == null) return;
        var item = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
        if (item == null) return;
        if (quantity <= 0)
            await _cartRepo.RemoveCartItemAsync(cartItemId);
        else
        {
            item.Quantity = quantity;
            await _cartRepo.UpdateCartItemAsync(item);
        }
    }

    public async Task ClearCartAsync(string userId)
    {
        var cart = await _cartRepo.GetCartByUserIdAsync(userId);
        if (cart != null) await _cartRepo.ClearCartAsync(cart.Id);
    }
}