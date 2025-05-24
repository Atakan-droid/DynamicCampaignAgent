using Data;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartItemsController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;

        public CartItemsController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetCartItems()
        {
            var cartItems = await _cartItemService.GetAllCartItemsAsync();
            return Ok(cartItems);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CartItem>> GetCartItem(Guid id)
        {
            var cartItem = await _cartItemService.GetCartItemByIdAsync(id);

            if (cartItem == null)
            {
                return NotFound();
            }

            return Ok(cartItem);
        }

        [HttpPost]
        public async Task<ActionResult<CartItem>> PostCartItem(CartItem cartItem)
        {
            var newCartItem = await _cartItemService.AddCartItemAsync(cartItem);
            return CreatedAtAction(nameof(GetCartItem), new { id = newCartItem.Id }, newCartItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartItem(Guid id, CartItem cartItem)
        {
            if (id != cartItem.Id)
            {
                return BadRequest();
            }

            var updatedCartItem = await _cartItemService.UpdateCartItemAsync(cartItem);

            if (updatedCartItem == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartItem(Guid id)
        {
            var result = await _cartItemService.DeleteCartItemAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}