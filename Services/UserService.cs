using Data.Entities;
using Services.Abstractions;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserProfile?> GetUserProfileAsync(string userId)
        {
            return await _userRepository.GetUserProfileAsync(userId);
        }
        public async Task<List<Session>> GetUserTransactionsAsync(string userId)
        {
            return await _userRepository.GetUserTransactionsAsync(userId);
        }
        public async Task UpdateUserProfileSummaryAsync(string userId)
        {
            var user = await _userRepository.GetUserProfileAsync(userId);
            if (user == null) return;
            var transactions = await GetUserTransactionsAsync(userId);
            var summary = $"User {user.Name} ({user.Email}) has made {transactions.Count} transactions.\n";
            foreach (var t in transactions)
            {
                summary += $"- {t.Timestamp:yyyy-MM-dd}: Campaigns {string.Join(",", t.TriggeredCampaigns)}, Basket Value: ${t.BasketValue}\n";
            }
            user.Summary = summary;
            await _userRepository.SaveChangesAsync();
        }
        public async Task<List<UserProfile>> GetAllUserProfilesAsync()
        {
            return await _userRepository.GetAllUserProfilesAsync();
        }
        public async Task<UserProfile?> AddUserAsync(UserProfile user)
        {
            return await _userRepository.AddUserAsync(user);
        }
        public async Task<List<UserProfile>> AddUsersAsync(List<UserProfile> users)
        {
            return await _userRepository.AddUsersAsync(users);
        }
        public async Task<UserProfile?> UpdateUserAsync(UserProfile user)
        {
            var existing = await _userRepository.GetUserProfileAsync(user.Id);
            if (existing == null) return null;

            existing.Name = user.Name;
            existing.Email = user.Email;
            existing.Summary = user.Summary;
            existing.TotalSpent = user.TotalSpent;
            existing.PurchaseCount = user.PurchaseCount;
            existing.LastPurchase = user.LastPurchase;

            return await _userRepository.UpdateUserAsync(existing);
        }
        public async Task<bool> DeleteUserAsync(string userId)
        {
            return await _userRepository.DeleteUserAsync(userId);
        }
    }
}