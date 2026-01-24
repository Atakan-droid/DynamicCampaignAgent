using Agents.CampaignAgent;
using Agents.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/agent")]
    public class AgentController : ControllerBase
    {
        private readonly CampaignAgent _campaignAgent;

        public AgentController(CampaignAgent campaignAgent)
        {
            _campaignAgent = campaignAgent;
        }

        [HttpPost("offer")]
        public async Task<IActionResult> GetOffer([FromBody] OfferRequest request)
        {
            var result = await _campaignAgent.OfferAsync(request);
            return Ok(result);
        }

        [HttpPost("check")]
        public async Task<IActionResult> CheckAndOffer([FromBody] CheckCampaignRequest request)
        {
            var result = await _campaignAgent.CheckCampaignsAsync(request);
            return Ok(result);
        }
    }
}