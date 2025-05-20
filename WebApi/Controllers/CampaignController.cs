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
        public CampaignController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
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
    }
} 