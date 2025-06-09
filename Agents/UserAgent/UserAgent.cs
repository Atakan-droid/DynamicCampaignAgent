using Microsoft.SemanticKernel;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;
using Agents.Models;
using Services;

namespace Agents.UserAgent
{
    public class UserAgent(Kernel _kernel, IUserService _userService)
    {
        public async Task<UserSummaryResult?> RunAsync(string userId)
        {
            var user = await _userService.GetUserProfileAsync(userId);
            if (user == null) return null;

            var transactions = await _userService.GetUserTransactionsAsync(userId);

            var profileSummary = $@"
User Profile:
- Name: {user.Name}
- Email: {user.Email}
- Total Spent: ${user.TotalSpent}
- Purchase Count: {user.PurchaseCount}
- Last Purchase: {(user.LastPurchase?.ToString("yyyy-MM-dd") ?? "N/A")}
- Coupon Count: {user.CouponCount}
";

            var transactionSummary = transactions.Any()
                ? string.Join("\n", transactions.Select(t =>
                    $"- {t.Timestamp:yyyy-MM-dd}: Campaigns {string.Join(",", t.TriggeredCampaigns)}, Basket Value: ${t.BasketValue}"))
                : "No transactions found.";

            var prompt = $@"
You are a customer insights AI. Summarize the following user's profile and transaction history. Return a JSON object with these fields:
- summary: string (a concise summary)
- topCampaigns: array of campaign names (most frequently triggered)
- avgBasketValue: number
- totalSpent: number
- purchaseCount: number
- lastPurchase: string (date)
- couponCount: number

{profileSummary}

Transaction History:
{transactionSummary}

Respond with only the JSON object.
";

            var result = await _kernel.InvokePromptAsync(prompt);
            var json = result.GetValue<string>() ?? "{}";
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<UserSummaryResult>(json, options);
        }
    }
}