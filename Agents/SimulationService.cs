using Data;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Agents
{
    public class SimulationService : ISimulationService
    {
        private readonly ApplicationDbContext _context;
        public SimulationService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task RecordCampaignSessionAsync(string userId, int campaignId, decimal basketValue)
        {
            var session = new CampaignSession
            {
                UserId = userId,
                CampaignId = campaignId,
                BasketValue = basketValue,
                Timestamp = DateTime.UtcNow
            };
            _context.CampaignSessions.Add(session);

            // Update user profile stats
            var user = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                user.TotalSpent += basketValue;
                user.PurchaseCount += 1;
                user.LastPurchase = session.Timestamp;
            }

            await _context.SaveChangesAsync();
        }
    }
} 