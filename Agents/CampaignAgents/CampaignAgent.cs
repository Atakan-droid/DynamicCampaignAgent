using Core.Models;
using Data.Enumerations;
using Microsoft.SemanticKernel;
using Services.Abstractions;
using System.Text.Json;

namespace Agents.CampaignAgents
{
    public class CampaignAgent(
        Kernel _kernel,
        IUserService _userService,
        ICampaignService _campaignService,
        ISessionTransactionService _sessionTransactionService,
        ICartItemService _cartItemService)
    {
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
                .Select(c => $"- {c.Name}: Rule: {c.Rule}, Effect: {c.Effect}"));

            var prompt = $@"
You are a campaign-negotiation AI agent. You dynamically select the best loyalty offers based on user profile, transaction history, active campaigns, and current cart items. Populate the responde schema:
- Offers: a list of CampaignOffer instances for each campaign that applies to the user's current basket only; include CampaignId, Description, CouponGiven, DiscountPercent, DiscountAmount, and ItemOffers.
- DiscountResult: the total basket discount amount and average percentage across the basket.
- Message: a concise summary of the combined offers.

User Profile:
- Name: {user.Name}
- Email: {user.Email}

Transaction History:
{string.Join("\n", transactions.Select(t => $"- {t.Timestamp:yyyy-MM-dd}: Campaigns {string.Join(",", t.TriggeredCampaigns)}, Basket Value: ${t.BasketValue}"))}

Active Campaigns:
{campaignDescriptions}

All Cart Items in System:
{allItemsText}

User's Current Basket Cart Items:
{cartItemsText}

User's Basket Total: ${request.CartItems.Sum(x => allCartItems.First(item => item.Id == x.Id).Price * x.Quantity)}

Respond with a single JSON object matching this schema:
{{
  'Offers': [
    'CampaignId': < int >, 'Description': '<text>', 'CouponGiven': < bool >, 'DiscountPercent': < decimal >, 'DiscountAmount': < decimal >, 'ItemOffers': [
        'CartItemId': '<guid>', 'DiscountPercent': < decimal >, 'DiscountAmount': < decimal >, 'Bonus': < bool >, 'BonusQuantity': < int >
    ] 
            
  ],
  'DiscountResult':  'TotalDiscountAmount': < decimal >, 'TotalDiscountPercent': < decimal > ,
  'Message': '<concise message>'
}}
Do not include any extra text.";

            var promptResult = await _kernel.InvokePromptAsync(prompt);
            var json = promptResult.GetValue<string>() ?? "{}";
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<OfferAgentResult>(json, options) ?? new OfferAgentResult { Message = "Parsing error" };

            var basketTotal = allCartItems
                .Where(item => request.CartItems.Any(x => x.Id == item.Id))
                .Sum(item => item.Price * request.CartItems.First(x => x.Id == item.Id)!.Quantity);

            if (result.Offers.Any())
            {
                await _sessionTransactionService.RecordSessionAsync(request.UserId, Guid.NewGuid(), basketTotal, result);
            }

            return result;
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
                return $"- {item.Name} (SKU: {item.SKU}, Quantity: {qty}, Price: ${item.Price})";
            }));

            var campaignDescriptions = string.Join("\n", campaigns
                .Where(c => c.Status == CampaignStatusTypes.Active)
                .Select(c => $"- {c.Name}: Rule: {c.Rule}, Effect: {c.Effect} (Status: {c.Status})"));

            var prompt = $@"
You are a campaign-negotiation AI agent. You dynamically analyze potential campaigns for the user based on profile, history, and cart.if you do X more spending, you qualify for campaign Y and you will gain Z. 

User Profile:
- Name: {user.Name}
- Email: {user.Email}

Transaction History:
{string.Join("\n", transactions.Select(t => $"- {t.Timestamp:yyyy-MM-dd}: Campaigns {string.Join(",", t.TriggeredCampaigns)}, Basket Value: ${t.BasketValue}"))}

Active Campaigns:
{campaignDescriptions}

All Basket Items in System:
{allItemsText}

User's Current Basket Items:
{cartItemsText}

User's Basket Total: ${request.CartItems.Sum(x => allCartItems.First(item => item.Id == x.Id).Price * x.Quantity)}

Respond with a single JSON object matching the CheckCampaignAgentResult schema:
{{
  ""AppliedOffers"": [
    {{ ""CampaignId"": <int>, ""Description"": ""<text>"", ""CouponGiven"": <bool>, ""DiscountPercent"": <decimal>, ""DiscountAmount"": <decimal>, ""ItemOffers"": [
        {{ ""CartItemId"": ""<guid>"", ""DiscountPercent"": <decimal>, ""DiscountAmount"": <decimal>, ""Bonus"": <bool>, ""BonusQuantity"": <int> }}
    ] }}
  ],
  ""AppliedDiscount"": {{ ""TotalDiscountAmount"": <decimal>, ""TotalDiscountPercent"": <decimal> }},
  ""Suggestions"": [
    {{
      ""Suggestion"": {{ ""CampaignId"": <int>, ""Description"": ""<text>"", ""CouponUse"": <bool>, ""CartItemSuggestions"": [
          {{ ""CartItemId"": ""<guid>"", ""Quantity"": <decimal> }}
      ] }},
      ""Offer"": {{ ""CampaignId"": <int>, ""Description"": ""<text>"", ""CouponGiven"": <bool>, ""DiscountPercent"": <decimal>, ""DiscountAmount"": <decimal>, ""ItemOffers"": [
             {{
            ""CartItemId"": ""<guid>"", ""DiscountPercent"": <decimal>, ""DiscountAmount"": <decimal>, ""Bonus"": <bool>, ""        BonusQuantity"":    <int>
            }}
        ] }}
    }}
  ],
  ""PotentialDiscountAfterSuggestion"": {{ ""TotalDiscountAmount"": <decimal>, ""TotalDiscountPercent"": <decimal> }}
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
