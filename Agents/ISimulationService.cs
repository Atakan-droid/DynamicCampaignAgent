using Data;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Agents
{
    public interface ISimulationService
    {
        Task RecordCampaignSessionAsync(string userId, int campaignId, decimal basketValue);
        Task<List<CampaignSession>> GetCampaignSessionsAsync();
        Task<List<CampaignSession>> GetCampaignSessionsByUserAsync(string userId);
    }
} 