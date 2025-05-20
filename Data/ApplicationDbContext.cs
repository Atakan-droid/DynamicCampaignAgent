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

            // Seed mock users with Guid.NewGuid() for Ids
            var user1Id = Guid.NewGuid().ToString();
            var user2Id = Guid.NewGuid().ToString();
            var user3Id = Guid.NewGuid().ToString();

            modelBuilder.Entity<UserProfile>().HasData(
                new UserProfile { Id = user1Id, Name = "Alice Smith", Email = "alice@example.com", TotalSpent = 120.50m, PurchaseCount = 1, LastPurchase = DateTime.Parse("2024-05-01T10:00:00Z") },
                new UserProfile { Id = user2Id, Name = "Bob Johnson", Email = "bob@example.com", TotalSpent = 75.00m, PurchaseCount = 1, LastPurchase = DateTime.Parse("2024-05-02T11:30:00Z") },
                new UserProfile { Id = user3Id, Name = "Charlie Lee", Email = "charlie@example.com", TotalSpent = 200.00m, PurchaseCount = 1, LastPurchase = DateTime.Parse("2024-05-03T09:15:00Z") }
            );

            modelBuilder.Entity<Campaign>().HasData(
                new Campaign { Id = 1, Name = "Spring Sale", IsActive = true, Rule = "Cart value > $500", Effect = "10% discount coupon" },
                new Campaign { Id = 2, Name = "Summer Bonanza", IsActive = true, Rule = "Buy 3 snacks, get 1 cola free", Effect = "Free cola" },
                new Campaign { Id = 3, Name = "Winter Clearance", IsActive = false, Rule = "Cart value > $1000", Effect = "20% discount coupon" },
                new Campaign { Id = 4, Name = "Gadget Weekend", IsActive = true, Rule = "Buy any 2 electronics, get 50% off on headphones", Effect = "50% off headphones" },
                new Campaign { Id = 5, Name = "Back to School", IsActive = true, Rule = "Buy 5 or more notebooks", Effect = "Free pen set" },
                new Campaign { Id = 6, Name = "Mega Grocery Deal", IsActive = true, Rule = "Cart contains at least 10 items", Effect = "$20 grocery voucher" },
                new Campaign { Id = 7, Name = "Fitness Frenzy", IsActive = true, Rule = "Buy a fitness tracker and any sportswear", Effect = "15% off next purchase" },
                new Campaign { Id = 8, Name = "Holiday Toys", IsActive = true, Rule = "Buy 2 toys, get 1 free", Effect = "Free toy (lowest price)" },
                new Campaign { Id = 9, Name = "Luxury Bonus", IsActive = true, Rule = "Cart value > $2000", Effect = "$200 luxury gift card" },
                new Campaign { Id = 10, Name = "Weekend Flash", IsActive = true, Rule = "Buy any item with SKU starting 'FLASH'", Effect = "Instant 5% off that item" }
            );

            modelBuilder.Entity<CampaignSession>().HasData(
                new CampaignSession { Id = 1, UserId = user1Id, CampaignId = 1, Timestamp = DateTime.Parse("2024-05-01T10:00:00Z"), BasketValue = 120.50m },
                new CampaignSession { Id = 2, UserId = user2Id, CampaignId = 2, Timestamp = DateTime.Parse("2024-05-02T11:30:00Z"), BasketValue = 75.00m },
                new CampaignSession { Id = 3, UserId = user3Id, CampaignId = 1, Timestamp = DateTime.Parse("2024-05-03T09:15:00Z"), BasketValue = 200.00m }
            );
        }
    }

    public class MockData
    {
        public List<UserProfile> Users { get; set; } = new();
        public List<Campaign> Campaigns { get; set; } = new();
        public List<CampaignSession> Transactions { get; set; } = new();
    }
} 