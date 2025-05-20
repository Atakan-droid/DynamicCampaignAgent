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
        public async Task<List<CampaignSession>> GetUserTransactionsAsync(string userId)
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
                summary += $"- {t.Timestamp:yyyy-MM-dd}: Campaign {t.CampaignId}, Basket Value: ${t.BasketValue}\n";
            }
            user.Summary = summary;
            await _context.SaveChangesAsync();
        }
        public async Task<List<UserProfile>> GetAllUserProfilesAsync()
        {
            return await _context.UserProfiles.ToListAsync();
        }
    }
} 