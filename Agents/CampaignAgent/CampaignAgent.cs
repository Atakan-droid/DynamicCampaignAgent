using Microsoft.SemanticKernel;
using Data.Enumerations;
using Data;
using Agents.Models;
using System.Text.Json;

namespace Agents.CampaignAgent
{
    public class CampaignAgent
    {
        private readonly Kernel _kernel;
        private readonly IUserService _userService;
        private readonly ICampaignService _campaignService;
        private readonly ISimulationService _simulationService;
        private readonly ICartItemService _cartItemService;

        public CampaignAgent(
            Kernel kernel,
            IUserService userService,
            ICampaignService campaignService,
            ISimulationService simulationService,
            ICartItemService cartItemService)
        {
            _kernel = kernel;
            _userService = userService;
            _campaignService = campaignService;
            _simulationService = simulationService;
            _cartItemService = cartItemService;
        }

        public async Task<OfferAgentResult> OfferAsync(OfferRequest request)
        {
            await _userService.UpdateUserProfileSummaryAsync(request.UserId);
            var user = await _userService.GetUserProfileAsync(request.UserId);
            var transactions = await _userService.GetUserTransactionsAsync(request.UserId);
            var allCartItems = await _cartItemService.GetAllCartItemsAsync();
            var campaigns = await _campaignService.GetActiveCampaignsAsync();

            if (user == null || campaigns.Count == 0)
                return new OfferAgentResult { Message = "No user or campaigns found." };

            var allItemsText = string.Join("\n", allCartItems.Select(item =>
                $"- Id: {item.Id}, SKU: {item.SKU}, Name: {item.Name}, Price: ${item.Price}"));

            var basketItems = allCartItems.Where(item => request.CartItems.Any(x => x.Id == item.Id)).ToList();
            var cartItemsText = string.Join("\n", basketItems.Select(item =>
            {
                var qty = request.CartItems.First(x => x.Id == item.Id)!.Quantity;
                return $"- {item.Name} (SKU: {item.SKU}, Qty: {qty}, Price: ${item.Price})";
            }));

            var campaignDescriptions = string.Join("\n", campaigns
                .Where(c => c.Status == CampaignStatusTypes.Active)
                .Select(c => $"- {c.Name}: Rule: {c.Rule}, Effect: {c.Effect} (Status: {c.Status})"));

            var prompt = $@"
You are a campaign-negotiation AI agent. You dynamically analyze potential campaigns for the user based on profile, history, and cart.

User Profile:
- Name: {user.Name}
- Email: {user.Email}

Transaction History:
{string.Join("\n", transactions.Select(t => $"- {t.Timestamp:yyyy-MM-dd}: Campaign {t.CampaignId}, Basket Value: ${t.BasketValue}"))}

Active Campaigns:
{campaignDescriptions}

All Cart Items in System:
{allItemsText}

User's Current Cart:
{cartItemsText}

Respond with a single JSON object matching this schema:
{{
  'CampaignId': ' < campaign - id - int > ',
    'DiscountResult':'TotalDiscountAmount': 0.0, 'TotalDiscountPercent': 0.0',
  'Coupon': false,
  'CouponCode': '<code-or-empty>',
  'Message': '<concise message>',
  'ItemOffers': [
     'CartItemId': '<guid>', 'DiscountPercent': 0.0, 'DiscountAmount': 0.0, 'Bonus': false, 'BonusQuantity': 0.0 
  ]
}}
Do not include any extra text.";

            var result = await _kernel.InvokePromptAsync(prompt);
            var json = result.GetValue<string>() ?? "{}";
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var offer = JsonSerializer.Deserialize<OfferAgentResult>(json, options)
                        ?? new OfferAgentResult { Message = "Parsing error" };

            var basketTotal = allCartItems
                .Where(item => request.CartItems.Any(x => x.Id == item.Id))
                .Sum(item => item.Price * request.CartItems.First(x => x.Id == item.Id)!.Quantity);

            await _simulationService.RecordCampaignSessionAsync(
                request.UserId,
                offer.CampaignId,
                basketTotal
                );

            return offer;
        }

        public async Task<CheckCampaignAgentResult> CheckCampaignsAsync(CheckCampaignRequest request)
        {
            await _userService.UpdateUserProfileSummaryAsync(request.UserId);
            var user = await _userService.GetUserProfileAsync(request.UserId);
            var transactions = await _userService.GetUserTransactionsAsync(request.UserId);
            var allCartItems = await _cartItemService.GetAllCartItemsAsync();
            var campaigns = await _campaignService.GetActiveCampaignsAsync();

            if (user == null || campaigns.Count == 0)
                return new CheckCampaignAgentResult();

            var allItemsText = string.Join("\n", allCartItems.Select(item =>
                $"- Id: {item.Id}, SKU: {item.SKU}, Name: {item.Name}, Price: ${item.Price}"));

            var basketItems = allCartItems.Where(item => request.CartItems.Any(x => x.Id == item.Id)).ToList();
            var cartItemsText = string.Join("\n", basketItems.Select(item =>
            {
                var qty = request.CartItems.First(x => x.Id == item.Id)!.Quantity;
                return $"- {item.Name} (SKU: {item.SKU}, Qty: {qty}, Price: ${item.Price})";
            }));

            var campaignDescriptions = string.Join("\n", campaigns
                .Where(c => c.Status == CampaignStatusTypes.Active)
                .Select(c => $"- {c.Name}: Rule: {c.Rule}, Effect: {c.Effect} (Status: {c.Status})"));

            var prompt = $@"
You are a campaign-negotiation AI agent. You dynamically analyze potential campaigns for the user based on profile, history, and cart.if you do X more spending, you qualify for campaign Y and you will gain Z

User Profile:
- Name: {user.Name}
- Email: {user.Email}

Transaction History:
{string.Join("\n", transactions.Select(t => $"- {t.Timestamp:yyyy-MM-dd}: Campaign {t.CampaignId}, Basket Value: ${t.BasketValue}"))}

Active Campaigns:
{campaignDescriptions}

All Cart Items in System:
{allItemsText}

User's Current Cart:
{cartItemsText}

Respond with a single JSON object matching this schema:
{{
  'Offers': [
            'CampaignId': '<int>', 'Description': '<text>', 'Coupon': false, 'DiscountPercent': 0.0, 'DiscountAmount': 0.0, 'Bonus': false, 'BonusQuantity': 0.0
  ],
  'PotentialDiscount': 'TotalDiscountAmount': 0.0, 'TotalDiscountPercent': 0.0,
  'Upgrades': [
    'CartItemId': '<guid>', 'Condition': '<text>', 'Coupon': false, 'DiscountPercent': 0.0, 'DiscountAmount': 0.0, 'Bonus': false, 'BonusQuantity': 0.0 
  ]
}}
Do not include any extra text.";

            var response = await _kernel.InvokePromptAsync(prompt);
            var jsonRes = response.GetValue<string>() ?? "{}";
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var checkResult = JsonSerializer.Deserialize<CheckCampaignAgentResult>(jsonRes, options)
                                ?? new CheckCampaignAgentResult();

            return checkResult;
        }
    }
}
