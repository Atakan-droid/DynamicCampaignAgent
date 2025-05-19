using Microsoft.SemanticKernel;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace Agents.CampaignAgent
{
    public class CampaignAgent
    {
        private readonly Kernel _kernel;
        private readonly IUserService _userService;
        private readonly ICampaignService _campaignService;
        private readonly ISimulationService _simulationService;

        public CampaignAgent(
            Kernel kernel,
            IUserService userService,
            ICampaignService campaignService,
            ISimulationService simulationService)
        {
            _kernel = kernel;
            _userService = userService;
            _campaignService = campaignService;
            _simulationService = simulationService;
        }

        public async Task<string> OfferAsync(string userId, List<CartItem> cartItems)
        {
            await _userService.UpdateUserProfileSummaryAsync(userId);
            var user = await _userService.GetUserProfileAsync(userId);
            var transactions = await _userService.GetUserTransactionsAsync(userId);
            var campaigns = await _campaignService.GetActiveCampaignsAsync();
            if (user == null || campaigns.Count == 0)
                return "No user or campaigns found.";

            var cartItemsText = string.Join("\n", cartItems.Select(item =>
                $"- {item.Name} (SKU: {item.SKU}, Qty: {item.Quantity}, Price: ${item.Price})"));

            var campaignDescriptions = string.Join("\n", campaigns.Select(c => $"- {c.Name}: Rule: {c.Rule}, Effect: {c.Effect} (Status: {(c.IsActive ? "Active" : "Inactive")})"));

            var prompt = $@"
User Profile:
- Name: {user.Name}
- Email: {user.Email}

Transaction History:
{string.Join("\n", transactions.Select(t => $"- {t.Timestamp:yyyy-MM-dd}: Campaign {t.CampaignId}, Basket Value: ${t.BasketValue}"))}

Active Campaigns:
{campaignDescriptions}

Cart Items:
{cartItemsText}

Respond in a single, concise sentence describing exactly what the user will gain (such as a discount, coupon code, voucher, or bonus item) if they proceed with this cart, or what they could gain by adding more items. Do not include any extra explanation.";
            var result = await _kernel.InvokePromptAsync(prompt);
            // Optionally record the session (pick a campaignId as needed)
            await _simulationService.RecordCampaignSessionAsync(userId, campaigns[0].Id, cartItems.Sum(i => i.Price * i.Quantity));
            return result.GetValue<string>() ?? string.Empty;
        }

        public async Task<string> CheckCampaignsAsync(string userId, List<CartItem> cartItems)
        {
            await _userService.UpdateUserProfileSummaryAsync(userId);
            var user = await _userService.GetUserProfileAsync(userId);
            var transactions = await _userService.GetUserTransactionsAsync(userId);
            var campaigns = await _campaignService.GetActiveCampaignsAsync();
            if (user == null || campaigns.Count == 0)
                return "No user or campaigns found.";

            var cartItemsText = string.Join("\n", cartItems.Select(item =>
                $"- {item.Name} (SKU: {item.SKU}, Qty: {item.Quantity}, Price: ${item.Price})"));

            var campaignDescriptions = string.Join("\n", campaigns.Select(c => $"- {c.Name}: Rule: {c.Rule}, Effect: {c.Effect} (Status: {(c.IsActive ? "Active" : "Inactive")})"));

            var prompt = $@"
User Profile:
- Name: {user.Name}
- Email: {user.Email}

Transaction History:
{string.Join("\n", transactions.Select(t => $"- {t.Timestamp:yyyy-MM-dd}: Campaign {t.CampaignId}, Basket Value: ${t.BasketValue}"))}

Active Campaigns:
{campaignDescriptions}

Cart Items:
{cartItemsText}

Respond in a single, concise sentence describing exactly what the user will gain (such as a discount, coupon code, voucher, or bonus item) if they proceed with this cart, or what they could gain by adding more items. Do not include any extra explanation.";
            var result = await _kernel.InvokePromptAsync(prompt);
            return result.GetValue<string>() ?? string.Empty;
        }
    }

    public class CartItem
    {
        public string Name { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
} 