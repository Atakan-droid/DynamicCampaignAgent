using Data.Entities;

namespace Services.Abstractions
{
    public interface ICampaignService
    {
        Task<List<Campaign>> GetActiveCampaignsAsync();
        Task<List<Campaign>> GetCampaignsAsync();
        Task<Campaign?> AddCampaignAsync(Campaign campaign);
        Task<Campaign?> UpdateCampaignAsync(Campaign campaign);
        Task<bool> DeleteCampaignAsync(int id);
    }
}