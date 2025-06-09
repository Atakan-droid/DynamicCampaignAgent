using Agents.Models;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Agents
{
    public sealed class SessionTransactionService(ApplicationDbContext _context) : ISessionTransactionService
    {
        public async Task RecordSessionAsync(string userId, Guid sessionId, decimal basketValue, OfferAgentResult result)
        {
            var session = new Session
            {
                UserId = userId,
                SessionId = sessionId,
                BasketValue = basketValue,
                Timestamp = DateTime.UtcNow,
                TriggeredCampaigns = result?.Offers.Select(o => o.CampaignId).Distinct().ToList() ?? [],
                Result = result != null ? System.Text.Json.JsonSerializer.Serialize(result) : null
            };

            _context.CampaignSessions.Add(session);

            var user = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                user.TotalSpent += basketValue;
                user.PurchaseCount += 1;
                user.CouponCount += result?.Offers.Count(o => o.CouponGiven) ?? 0;
                user.LastPurchase = session.Timestamp;
            }

            await _context.SaveChangesAsync();
        }
        public async Task<List<Session>> GetSessionsAsync()
        {
            return await _context.CampaignSessions.ToListAsync();
        }
        public async Task<List<Session>> GetSessionsByUserAsync(string userId)
        {
            return await _context.CampaignSessions.Where(s => s.UserId == userId).ToListAsync();
        }
    }
} 