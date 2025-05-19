using Agents.CampaignAgent;
using Agents.UserAgent;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/agent")]
    public class AgentController : ControllerBase
    {
        private readonly UserAgent _userAgent;
        private readonly CampaignAgent _campaignAgent;

        public AgentController(UserAgent userAgent, CampaignAgent campaignAgent)
        {
            _userAgent = userAgent;
            _campaignAgent = campaignAgent;
        }

        [HttpPost("profile")]
        public async Task<IActionResult> GetProfile([FromBody] ProfileRequest request)
        {
            var result = await _userAgent.RunAsync(request.UserId);
            return Ok(result);
        }

        [HttpPost("offer")]
        public async Task<IActionResult> GetOffer([FromBody] OfferRequest request)
        {
            var result = await _campaignAgent.OfferAsync(request.UserId, request.CartItems);
            return Ok(result);
        }

        [HttpPost("check")]
        public async Task<IActionResult> CheckAndOffer([FromBody] CheckRequest request)
        {
            var result = await _campaignAgent.CheckCampaignsAsync(request.UserId, request.CartItems);
            return Ok(result);
        }
    }

    public class ProfileRequest
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class OfferRequest
    {
        public string UserId { get; set; } = string.Empty;
        public List<CartItem> CartItems { get; set; } = new();
    }

    public class CheckRequest
    {
        public string UserId { get; set; } = string.Empty;
        public List<CartItem> CartItems { get; set; } = new();
    }

} 