using Data;
using Microsoft.EntityFrameworkCore;
using Services.Abstractions;

namespace Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfile?> GetUserProfileAsync(string userId)
        {
            return await _context.UserProfiles.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<Session>> GetUserTransactionsAsync(string userId)
        {
            return await _context.CampaignSessions.Where(t => t.UserId == userId).ToListAsync();
        }

        public async Task<List<UserProfile>> GetAllUserProfilesAsync()
        {
            return await _context.UserProfiles.ToListAsync();
        }

        public async Task<UserProfile> AddUserAsync(UserProfile user)
        {
            _context.UserProfiles.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<List<UserProfile>> AddUsersAsync(List<UserProfile> users)
        {
            _context.UserProfiles.AddRange(users);
            await _context.SaveChangesAsync();
            return users;
        }

        public async Task<UserProfile?> UpdateUserAsync(UserProfile user)
        {
            var existing = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return false;

            _context.UserProfiles.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
