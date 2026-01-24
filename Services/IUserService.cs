using Data;

namespace Services
{
    public interface IUserService
    {
        Task<UserProfile?> GetUserProfileAsync(string userId);
        Task<List<Session>> GetUserTransactionsAsync(string userId);
        Task UpdateUserProfileSummaryAsync(string userId);
        Task<List<UserProfile>> GetAllUserProfilesAsync();
        Task<UserProfile?> AddUserAsync(UserProfile user);
        Task<List<UserProfile>> AddUsersAsync(List<UserProfile> users);
        Task<UserProfile?> UpdateUserAsync(UserProfile user);
        Task<bool> DeleteUserAsync(string userId);
    }
}