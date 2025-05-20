namespace Data
{
    public class UserProfile
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Summary { get; set; } // LLM context summary
        public decimal TotalSpent { get; set; }
        public int PurchaseCount { get; set; }
        public DateTime? LastPurchase { get; set; }
    }
} 