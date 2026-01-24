namespace Core.Models
{
    public class UserSummaryResult
    {
        public string Summary { get; set; } = string.Empty;
        public List<string> TopCampaigns { get; set; } = new();
        public decimal AvgBasketValue { get; set; }
        public decimal TotalSpent { get; set; }
        public int PurchaseCount { get; set; }
        public string? LastPurchase { get; set; }
        public int CouponCount { get; set; }
    }
}