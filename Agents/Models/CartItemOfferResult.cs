namespace Agents.Models
{
    public class CartItemOfferResult
    {
        public Guid CartItemId { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool Bonus { get; set; }
        public int BonusQuantity { get; set; }
    }
}
