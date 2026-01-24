namespace Core.Models
{
    public class CartItemUpgradeSuggestionResult
    {
        public Guid CartItemId { get; set; }
        public string Condition { get; set; } = string.Empty;
        public bool Coupon { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool Bonus { get; set; }
        public decimal BonusQuantity { get; set; }
    }
}
