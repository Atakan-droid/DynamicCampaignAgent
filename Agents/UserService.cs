using Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Agents
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<UserProfile?> GetUserProfileAsync(string userId)
        {
            return await _context.UserProfiles.FirstOrDefaultAsync(u => u.Id == userId);
        }
        public async Task<List<Session>> GetUserTransactionsAsync(string userId)
        {
            return await _context.CampaignSessions.Where(t => t.UserId == userId).ToListAsync();
        }
        public async Task UpdateUserProfileSummaryAsync(string userId)
        {
            var user = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return;
            var transactions = await GetUserTransactionsAsync(userId);
            var summary = $"User {user.Name} ({user.Email}) has made {transactions.Count} transactions.\n";
            foreach (var t in transactions)
            {
                summary += $"- {t.Timestamp:yyyy-MM-dd}: Campaigns {string.Join(",", t.TriggeredCampaigns)}, Basket Value: ${t.BasketValue}\n";
            }
            user.Summary = summary;
            await _context.SaveChangesAsync();
        }
        public async Task<List<UserProfile>> GetAllUserProfilesAsync()
        {
            return await _context.UserProfiles.ToListAsync();
        }
        public async Task<UserProfile?> AddUserAsync(UserProfile user)
        {
            _context.UserProfiles.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<List<UserProfile>> AddUsersAsync(List<UserProfile> users)
        {
            _context.UserProfiles.AddRange(users);
            await _context.SaveChangesAsync();
            return users;
        }
        public async Task<UserProfile?> UpdateUserAsync(UserProfile user)
        {
            var existing = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (existing == null) return null;
            existing.Name = user.Name;
            existing.Email = user.Email;
            existing.Summary = user.Summary;
            existing.TotalSpent = user.TotalSpent;
            existing.PurchaseCount = user.PurchaseCount;
            existing.LastPurchase = user.LastPurchase;
            await _context.SaveChangesAsync();
            return existing;
        }
        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return false;
            _context.UserProfiles.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}