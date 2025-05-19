using Data;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Agents
{
    public interface IUserService
    {
        Task<UserProfile?> GetUserProfileAsync(string userId);
        Task<List<CampaignSession>> GetUserTransactionsAsync(string userId);
        Task UpdateUserProfileSummaryAsync(string userId);
    }
} 