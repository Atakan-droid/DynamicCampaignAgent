namespace Core.Models
{
    public sealed class CartItemRequest
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
    }
}
