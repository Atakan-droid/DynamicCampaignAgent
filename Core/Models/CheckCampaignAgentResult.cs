namespace Core.Models
{
    public class CheckCampaignAgentResult
    {
        public List<CampaignOffer> AppliedOffers { get; set; } = new List<CampaignOffer>();
        public DiscountResult? AppliedDiscount { get; set; }
        public List<BasketUpgradeSuggestionResult> Suggestions { get; set; } = [];
        public DiscountResult? PotentialDiscountAfterSuggestion { get; set; }
    }

    public sealed class BasketUpgradeSuggestionResult
    {
        public CampaignSuggestion Suggestion { get; set; } = new CampaignSuggestion();
        public CampaignOffer Offer { get; set; } = new CampaignOffer();
    }

    public sealed class CampaignSuggestion
    {
        public int CampaignId { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool CouponUse { get; set; }
        public List<CartItemSuggestion> CartItemSuggestions { get; set; } = [];
    }

    public sealed class CartItemSuggestion
    {
        public Guid CartItemId { get; set; }
        public decimal Quantity { get; set; }
    }
}
