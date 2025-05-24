using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Data.Enumerations;

namespace Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
        public DbSet<Campaign> Campaigns => Set<Campaign>();
        public DbSet<CampaignSession> CampaignSessions => Set<CampaignSession>();
        public DbSet<CartItem> CartItems => Set<CartItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserProfile>().HasData(
                new UserProfile { Id = "f4079fc8-cb2d-4a63-8698-6beba78d843e", Name = "Alice Smith", Email = "alice@example.com"},
                new UserProfile { Id = "855092ce-f114-49ab-b664-02c017a3ff74", Name = "Bob Johnson", Email = "bob@example.com"},
                new UserProfile { Id = "b10b003f-fe53-435d-9bb4-523bebe92906", Name = "Charlie Lee", Email = "charlie@example.com"}
            );

            modelBuilder.Entity<Campaign>().HasData(
                new Campaign { Id = 1, Name = "Spring Sale", Status = CampaignStatusTypes.Active, Rule = "Cart value > $500", Effect = "10% discount coupon", TotalBudget = 10000, MaxBudgetPerUser = 500, CampaignTarget = "Increase spring sales" },
                new Campaign { Id = 2, Name = "Summer Bonanza", Status = CampaignStatusTypes.Active, Rule = "Buy 3 snacks, get 1 cola free", Effect = "Free cola", TotalBudget = 8000, MaxBudgetPerUser = 300, CampaignTarget = "Boost snack sales" },
                new Campaign { Id = 3, Name = "Winter Clearance", Status = CampaignStatusTypes.Completed, Rule = "Cart value > $1000", Effect = "20% discount coupon", TotalBudget = 5000, MaxBudgetPerUser = 1000, CampaignTarget = "Clear winter stock" },
                new Campaign { Id = 4, Name = "Gadget Weekend", Status = CampaignStatusTypes.Active, Rule = "Buy any 2 electronics, get 50% off on headphones", Effect = "50% off headphones", TotalBudget = 12000, MaxBudgetPerUser = 600, CampaignTarget = "Promote electronics" },
                new Campaign { Id = 5, Name = "Back to School", Status = CampaignStatusTypes.Active, Rule = "Buy 5 or more notebooks", Effect = "Free pen set", TotalBudget = 4000, MaxBudgetPerUser = 200, CampaignTarget = "School supplies push" },
                new Campaign { Id = 6, Name = "Mega Grocery Deal", Status = CampaignStatusTypes.Active, Rule = "Cart contains at least 10 items", Effect = "$20 grocery voucher", TotalBudget = 15000, MaxBudgetPerUser = 700, CampaignTarget = "Increase grocery basket size" },
                new Campaign { Id = 7, Name = "Fitness Frenzy", Status = CampaignStatusTypes.Active, Rule = "Buy a fitness tracker and any sportswear", Effect = "15% off next purchase", TotalBudget = 6000, MaxBudgetPerUser = 350, CampaignTarget = "Promote fitness products" },
                new Campaign { Id = 8, Name = "Holiday Toys", Status = CampaignStatusTypes.Active, Rule = "Buy 2 toys, get 1 free", Effect = "Free toy (lowest price)", TotalBudget = 9000, MaxBudgetPerUser = 400, CampaignTarget = "Holiday toy sales" },
                new Campaign { Id = 9, Name = "Luxury Bonus", Status = CampaignStatusTypes.Active, Rule = "Cart value > $2000", Effect = "$200 luxury gift card", TotalBudget = 20000, MaxBudgetPerUser = 2000, CampaignTarget = "Luxury segment growth" },
                new Campaign { Id = 10, Name = "Weekend Flash", Status = CampaignStatusTypes.Active, Rule = "Buy any item with SKU starting 'FLASH'", Effect = "Instant 5% off that item", TotalBudget = 3000, MaxBudgetPerUser = 150, CampaignTarget = "Promote flash deals" }
            );

            modelBuilder.Entity<CartItem>().HasData(
                new CartItem { Id = Guid.Parse("bd3e8919-bbd2-4466-a9c4-5c9a30f3f845"), Name = "Product 1", SKU = "SKU123", Price = 100 },
                new CartItem { Id = Guid.Parse("ca02e081-56bc-414d-b7f9-f08bb236e19b"), Name = "Product 2", SKU = "SKU456", Price = 200 },
                new CartItem { Id = Guid.Parse("9034188e-76ec-44a8-8ae6-422c18f51365"), Name = "Product 3", SKU = "SKU789", Price = 300 }
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