using Data.DbContexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Abstractions;

namespace Services
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
            return await _context.Campaigns.Where(c => c.Status == Data.Enumerations.CampaignStatusTypes.Active).ToListAsync();
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
            existing.Status = campaign.Status;
            existing.Rule = campaign.Rule;
            existing.Effect = campaign.Effect;
            existing.CampaignTarget = campaign.CampaignTarget;
            await _context.SaveChangesAsync();
            return existing;
        }
        public async Task<bool> DeleteCampaignAsync(int id)
        {
            var campaign = await _context.Campaigns.FirstOrDefaultAsync(c => c.Id == id);
            if (campaign == null) return false;
            _context.Campaigns.Remove(campaign);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}