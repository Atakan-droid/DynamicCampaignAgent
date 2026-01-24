using Agents;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/campaign")]
    public class CampaignController(ICampaignService _campaignService, ISessionTransactionService _sessionTransactionService) : ControllerBase
    {
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCampaign(int id)
        {
            var result = await _campaignService.DeleteCampaignAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("sessions")]
        public async Task<ActionResult<List<Session>>> GetCampaignSessions([FromQuery] string? userId = null)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var userSessions = await _sessionTransactionService.GetSessionsByUserAsync(userId);
                return Ok(userSessions);
            }
            var allSessions = await _sessionTransactionService.GetSessionsAsync();
            return Ok(allSessions);
        }
    }
}