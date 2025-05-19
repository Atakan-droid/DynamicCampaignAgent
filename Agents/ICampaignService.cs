using Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agents
{
    public interface ICampaignService
    {
        Task<List<Campaign>> GetActiveCampaignsAsync();
    }
} 