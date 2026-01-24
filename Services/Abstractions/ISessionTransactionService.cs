using Core.Models;
using Data.Entities;

namespace Services.Abstractions
{
    public interface ISessionTransactionService
    {
        Task RecordSessionAsync(string userId, Guid sessionId, decimal basketValue, OfferAgentResult result);
        Task<List<Session>> GetSessionsAsync();
        Task<List<Session>> GetSessionsByUserAsync(string userId);
    }
}