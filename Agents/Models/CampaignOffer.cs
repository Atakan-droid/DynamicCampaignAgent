namespace Agents.Models
{
    public class CampaignOffer
    {
        public int CampaignId { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool Coupon { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool Bonus { get; set; }
        public decimal BonusQuantity { get; set; }
    }
}
