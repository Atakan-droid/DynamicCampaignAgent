using Data;

namespace Agents.Models
{
    public class OfferAgentResult
    {
        public List<CampaignOffer> Offers { get; set; } = [];
        public DiscountResult DiscountResult { get; set; } = new();
        public string Message { get; set; } = string.Empty;
    }
}
