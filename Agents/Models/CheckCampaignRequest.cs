namespace Agents.Models
{
    public sealed class CheckCampaignRequest
    {
        public string UserId { get; set; } = string.Empty;
        public List<CartItemRequest> CartItems { get; set; } = [];
    }
}
