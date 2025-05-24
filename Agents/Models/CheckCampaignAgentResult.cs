namespace Agents.Models
{
    public class CheckCampaignAgentResult
    {
        public List<CampaignOffer> Offers { get; set; } = new List<CampaignOffer>();
        public DiscountResult? PotentialDiscount { get; set; } // Sepet tamamlandığında tahmini indirim
        public List<CartItemUpgradeSuggestionResult> Upgrades { get; set; } = new List<CartItemUpgradeSuggestionResult>();
    }
}
