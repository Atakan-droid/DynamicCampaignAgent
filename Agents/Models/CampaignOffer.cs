namespace Agents.Models
{
    public class CampaignOffer
    {
        public int CampaignId { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool CouponGiven { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public List<CartItemOfferResult> ItemOffers { get; set; } = [];
    }
}
