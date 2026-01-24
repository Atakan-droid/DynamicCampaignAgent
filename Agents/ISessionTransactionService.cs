using Agents.Models;
using Data;

namespace Agents
{
    public interface ISessionTransactionService
    {
        Task RecordSessionAsync(string userId, Guid sessionId, decimal basketValue, OfferAgentResult result);
        Task<List<Session>> GetSessionsAsync();
        Task<List<Session>> GetSessionsByUserAsync(string userId);
    }
}