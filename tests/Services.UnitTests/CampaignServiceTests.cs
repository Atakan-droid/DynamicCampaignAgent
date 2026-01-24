using Data;
using Data.Enumerations;
using FluentAssertions;
using Moq;
using Services;
using Services.Abstractions;
using Services.UnitTests.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Services.UnitTests;

public class CampaignServiceTests
{
    private readonly Mock<ICampaignRepository> _mockCampaignRepository;
    private readonly CampaignService _sut;

    public CampaignServiceTests()
    {
        _mockCampaignRepository = new Mock<ICampaignRepository>();
        _sut = new CampaignService(_mockCampaignRepository.Object);
    }

    [Fact]
    public async Task GetActiveCampaignsAsync_WhenActiveCampaignsExist_ReturnsOnlyActiveCampaigns()
    {
        // Arrange
        var campaigns = TestFaker.CreateCampaignList();
        var activeCampaigns = campaigns.Where(c => c.Status == CampaignStatusTypes.Active).ToList();
        _mockCampaignRepository.Setup(r => r.GetActiveCampaignsAsync()).ReturnsAsync(activeCampaigns);

        // Act
        var result = await _sut.GetActiveCampaignsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(activeCampaigns);
        result.Should().OnlyContain(c => c.Status == CampaignStatusTypes.Active);
    }

    [Fact]
    public async Task GetCampaignsAsync_WhenCalled_ReturnsAllCampaigns()
    {
        // Arrange
        var campaigns = TestFaker.CreateCampaignList(5);
        _mockCampaignRepository.Setup(r => r.GetCampaignsAsync()).ReturnsAsync(campaigns);

        // Act
        var result = await _sut.GetCampaignsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(5);
    }

    [Fact]
    public async Task GetCampaignsAsync_WhenNoCampaignsExist_ReturnsEmptyList()
    {
        // Arrange
        _mockCampaignRepository.Setup(r => r.GetCampaignsAsync()).ReturnsAsync(new List<Campaign>());

        // Act
        var result = await _sut.GetCampaignsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task AddCampaignAsync_WithValidCampaign_ReturnsCampaign()
    {
        // Arrange
        var newCampaign = TestFaker.CreateCampaign();
        _mockCampaignRepository.Setup(r => r.AddCampaignAsync(newCampaign)).ReturnsAsync(newCampaign);

        // Act
        var result = await _sut.AddCampaignAsync(newCampaign);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(newCampaign);
    }

    [Fact]
    public async Task UpdateCampaignAsync_WhenCampaignExists_UpdatesAndReturnsCampaign()
    {
        // Arrange
        var campaignToUpdate = TestFaker.CreateCampaign();
        _mockCampaignRepository.Setup(r => r.UpdateCampaignAsync(campaignToUpdate)).ReturnsAsync(campaignToUpdate);

        // Act
        var result = await _sut.UpdateCampaignAsync(campaignToUpdate);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(campaignToUpdate);
    }

    [Fact]
    public async Task UpdateCampaignAsync_WithNonExistentCampaign_ReturnsNull()
    {
        // Arrange
        var campaignToUpdate = TestFaker.CreateCampaign();
        _mockCampaignRepository.Setup(r => r.UpdateCampaignAsync(campaignToUpdate)).ReturnsAsync((Campaign?)null);

        // Act
        var result = await _sut.UpdateCampaignAsync(campaignToUpdate);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteCampaignAsync_WhenCampaignExists_ReturnsTrue()
    {
        // Arrange
        _mockCampaignRepository.Setup(r => r.DeleteCampaignAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _sut.DeleteCampaignAsync(1);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteCampaignAsync_WhenCampaignDoesNotExist_ReturnsFalse()
    {
        // Arrange
        _mockCampaignRepository.Setup(r => r.DeleteCampaignAsync(99)).ReturnsAsync(false);

        // Act
        var result = await _sut.DeleteCampaignAsync(99);

        // Assert
        result.Should().BeFalse();
    }
}
