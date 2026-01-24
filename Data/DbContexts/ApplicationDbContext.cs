using Data.Entities;
using Data.Enumerations;
using Microsoft.EntityFrameworkCore;

namespace Data.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<Campaign> Campaigns { get; set; }
        public virtual DbSet<Session> CampaignSessions { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserProfile>().HasData(
                new UserProfile { Id = "f4079fc8-cb2d-4a63-8698-6beba78d843e", Name = "Alice Smith", Email = "alice@example.com" },
                new UserProfile { Id = "855092ce-f114-49ab-b664-02c017a3ff74", Name = "Bob Johnson", Email = "bob@example.com" },
                new UserProfile { Id = "b10b003f-fe53-435d-9bb4-523bebe92906", Name = "Charlie Lee", Email = "charlie@example.com" }
            );

            modelBuilder.Entity<Campaign>().HasData(
    new Campaign { Id = 1, Name = "Spring Sale", Status = CampaignStatusTypes.Active, Rule = "Basket Total is greater than or equal $500", Effect = "Apply 10% off discount", CampaignTarget = "Increase spring sales" },
    new Campaign { Id = 2, Name = "Summer Bonanza", Status = CampaignStatusTypes.Active, Rule = "Buy 3 of SKU is 'SKU456'", Effect = "Get 1 free 'SKU123' item", CampaignTarget = "Boost snack sales" }
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
        public List<Session> Transactions { get; set; } = new();
    }
}