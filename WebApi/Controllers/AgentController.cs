using Agents.CampaignAgent;
using Agents.UserAgent;
using Microsoft.AspNetCore.Mvc;
using Agents;
using Data;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/agent")]
    public class AgentController : ControllerBase
    {
        private readonly UserAgent _userAgent;
        private readonly CampaignAgent _campaignAgent;
        private readonly IUserService _userService;

        public AgentController(UserAgent userAgent, CampaignAgent campaignAgent, IUserService userService)
        {
            _userAgent = userAgent;
            _campaignAgent = campaignAgent;
            _userService = userService;
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

        [HttpGet("users")]
        public async Task<IActionResult> ListUsers()
        {
            var users = await _userService.GetAllUserProfilesAsync();
            return Ok(users);
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