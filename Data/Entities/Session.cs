namespace Data.Entities
{
    public class Session
    {
        public int Id { get; set; }
        public Guid SessionId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public decimal BasketValue { get; set; }
        public DateTime Timestamp { get; set; }
        public List<int> TriggeredCampaigns { get; set; } = [];
        public string? Result { get; set; }
    }
}