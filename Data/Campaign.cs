namespace Data
{
    public class Campaign
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string Rule { get; set; } = string.Empty; // e.g. "Cart value > $500"
        public string Effect { get; set; } = string.Empty; // e.g. "10% discount coupon"
    }
} 