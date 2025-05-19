using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
        public DbSet<Campaign> Campaigns => Set<Campaign>();
        public DbSet<CampaignSession> CampaignSessions => Set<CampaignSession>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Load mock data from JSON file
            var mockDataPath = Path.Combine(Environment.CurrentDirectory, "..", "Data", "MockData.json");
            if (File.Exists(mockDataPath))
            {
                var json = File.ReadAllText(mockDataPath);
                var mock = JsonSerializer.Deserialize<MockData>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (mock != null)
                {
                    modelBuilder.Entity<UserProfile>().HasData(mock.Users);
                    modelBuilder.Entity<Campaign>().HasData(mock.Campaigns);
                    modelBuilder.Entity<CampaignSession>().HasData(mock.Transactions);
                }
            }
        }
    }

    public class MockData
    {
        public List<UserProfile> Users { get; set; } = new();
        public List<Campaign> Campaigns { get; set; } = new();
        public List<CampaignSession> Transactions { get; set; } = new();
    }
} 