using Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agents
{
    public interface ICampaignService
    {
        Task<List<Campaign>> GetActiveCampaignsAsync();
        Task<List<Campaign>> GetCampaignsAsync();
        Task<Campaign?> AddCampaignAsync(Campaign campaign);
        Task<Campaign?> UpdateCampaignAsync(Campaign campaign);
        Task<bool> DeleteCampaignAsync(string id);
    }
} 