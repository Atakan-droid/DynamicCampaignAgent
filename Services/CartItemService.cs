using Data.DbContexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Abstractions;

namespace Services
{
    public class CartItemService : ICartItemService
    {
        private readonly ApplicationDbContext _context;

        public CartItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CartItem>> GetAllCartItemsAsync()
        {
            return await _context.CartItems.ToListAsync();
        }

        public async Task<CartItem?> GetCartItemByIdAsync(Guid id) => await _context.CartItems.FindAsync(id);

        public async Task<CartItem> AddCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<CartItem?> UpdateCartItemAsync(CartItem cartItem)
        {
            _context.Entry(cartItem).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CartItemExists(cartItem.Id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
            return cartItem;
        }

        public async Task<bool> DeleteCartItemAsync(Guid id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                return false;
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> CartItemExists(Guid id)
        {
            return await _context.CartItems.AnyAsync(e => e.Id == id);
        }
    }
}