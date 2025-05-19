using System;

namespace Data
{
    public class CampaignSession
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int CampaignId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal BasketValue { get; set; }
    }
} 