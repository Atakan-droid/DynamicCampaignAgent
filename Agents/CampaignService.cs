using Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agents
{
    public class CampaignService : ICampaignService
    {
        private readonly ApplicationDbContext _context;
        public CampaignService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Campaign>> GetActiveCampaignsAsync()
        {
            return await _context.Campaigns.Where(c => c.IsActive).ToListAsync();
        }
    }
} 