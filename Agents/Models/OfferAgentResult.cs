using Data;

namespace Agents.Models
{
    public class OfferAgentResult
    {
        public int CampaignId { get; set; }
        public DiscountResult DiscountResult { get; set; } = new DiscountResult();
        public bool Coupon { get; set; }
        public string CouponCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public List<CartItemOfferResult> ItemOffers { get; set; } = new List<CartItemOfferResult>();
    }
}
