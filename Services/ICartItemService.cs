using Data;
using System.Collections.Generic;
namespace Services
{
    public interface ICartItemService
    {
        Task<IEnumerable<CartItem>> GetAllCartItemsAsync();
        Task<CartItem?> GetCartItemByIdAsync(Guid id);
        Task<CartItem> AddCartItemAsync(CartItem cartItem);
        Task<CartItem?> UpdateCartItemAsync(CartItem cartItem);
        Task<bool> DeleteCartItemAsync(Guid id);
    }
}