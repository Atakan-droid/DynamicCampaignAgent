using Microsoft.AspNetCore.Mvc;
using Agents;
using Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/campaign")]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;
        private readonly ISimulationService _simulationService;
        public CampaignController(ICampaignService campaignService, ISimulationService simulationService)
        {
            _campaignService = campaignService;
            _simulationService = simulationService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Campaign>>> GetCampaigns()
        {
            var campaigns = await _campaignService.GetCampaignsAsync();
            return Ok(campaigns);
        }

        [HttpPost]
        public async Task<ActionResult<Campaign>> AddCampaign([FromBody] Campaign campaign)
        {
            var result = await _campaignService.AddCampaignAsync(campaign);
            return CreatedAtAction(nameof(GetCampaigns), new { id = result?.Id }, result);
        }

        [HttpPut]
        public async Task<ActionResult<Campaign>> UpdateCampaign([FromBody] Campaign campaign)
        {
            var result = await _campaignService.UpdateCampaignAsync(campaign);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("sessions")]
        public async Task<ActionResult<List<CampaignSession>>> GetCampaignSessions([FromQuery] string? userId = null)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var userSessions = await _simulationService.GetCampaignSessionsByUserAsync(userId);
                return Ok(userSessions);
            }
            var allSessions = await _simulationService.GetCampaignSessionsAsync();
            return Ok(allSessions);
        }
    }
} 