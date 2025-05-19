using Microsoft.SemanticKernel;
using System.Threading.Tasks;

namespace Agents.UserAgent
{
    public class UserAgent
    {
        private readonly Kernel _kernel;
        private readonly IUserService _userService;

        public UserAgent(Kernel kernel, IUserService userService)
        {
            _kernel = kernel;
            _userService = userService;
        }

        public async Task<string> RunAsync(string userId)
        {
            var user = await _userService.GetUserProfileAsync(userId);
            if (user == null) return $"User {userId} not found.";
            var prompt = $"Summarize the profile for user: Name={user.Name}, Email={user.Email}";
            var result = await _kernel.InvokePromptAsync(prompt);
            return result.GetValue<string>() ?? string.Empty;
        }
    }
} 