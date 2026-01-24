namespace Core.Models
{
    public sealed class OfferRequest
    {
        public string UserId { get; set; } = string.Empty;
        public List<CartItemRequest> CartItems { get; set; } = [];
    }
}
