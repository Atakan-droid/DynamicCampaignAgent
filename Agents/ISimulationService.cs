using Data;
using System.Threading.Tasks;

namespace Agents
{
    public interface ISimulationService
    {
        Task RecordCampaignSessionAsync(string userId, int campaignId, decimal basketValue);
    }
} 