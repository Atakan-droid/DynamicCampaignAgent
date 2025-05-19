using Data;
using System;
using System.Threading.Tasks;

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
            await _context.SaveChangesAsync();
        }
    }
} 