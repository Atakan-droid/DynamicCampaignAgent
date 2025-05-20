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

        public async Task<List<Campaign>> GetCampaignsAsync()
        {
            return await _context.Campaigns.ToListAsync();
        }

        public async Task<Campaign?> AddCampaignAsync(Campaign campaign)
        {
            _context.Campaigns.Add(campaign);
            await _context.SaveChangesAsync();
            return campaign;
        }

        public async Task<Campaign?> UpdateCampaignAsync(Campaign campaign)
        {
            var existing = await _context.Campaigns.FirstOrDefaultAsync(c => c.Id == campaign.Id);
            if (existing == null) return null;
            existing.Name = campaign.Name;
            existing.IsActive = campaign.IsActive;
            existing.Rule = campaign.Rule;
            existing.Effect = campaign.Effect;
            existing.TotalBudget = campaign.TotalBudget;
            existing.MaxBudgetPerUser = campaign.MaxBudgetPerUser;
            existing.CampaignTarget = campaign.CampaignTarget;
            await _context.SaveChangesAsync();
            return existing;
        }
    }
} 