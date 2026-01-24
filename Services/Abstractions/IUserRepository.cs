using Data.Entities;

namespace Services.Abstractions
{
    public interface IUserRepository
    {
        Task<UserProfile?> GetUserProfileAsync(string userId);
        Task<List<Session>> GetUserTransactionsAsync(string userId);
        Task<List<UserProfile>> GetAllUserProfilesAsync();
        Task<UserProfile> AddUserAsync(UserProfile user);
        Task<List<UserProfile>> AddUsersAsync(List<UserProfile> users);
        Task<UserProfile?> UpdateUserAsync(UserProfile user);
        Task<bool> DeleteUserAsync(string userId);
        Task SaveChangesAsync();
    }
}
