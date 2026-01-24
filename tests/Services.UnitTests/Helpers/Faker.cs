using AutoBogus;
using Bogus;
using Data;
using FluentAssertions;
using Moq;
using Services.Abstractions;
using Services.UnitTests.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Services.UnitTests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _sut;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _sut = new UserService(_mockUserRepository.Object);
        }

        [Fact]
        public async Task GetUserProfileAsync_WhenUserExists_ReturnsUserProfile()
        {
            // Arrange
            var userProfile = TestFaker.CreateUserProfile();
            _mockUserRepository.Setup(r => r.GetUserProfileAsync(userProfile.Id)).ReturnsAsync(userProfile);

            // Act
            var result = await _sut.GetUserProfileAsync(userProfile.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(userProfile);
        }

        [Fact]
        public async Task GetUserProfileAsync_WhenUserDoesNotExist_ReturnsNull()
        {
            // Arrange
            _mockUserRepository.Setup(r => r.GetUserProfileAsync(It.IsAny<string>())).ReturnsAsync((UserProfile?)null);

            // Act
            var result = await _sut.GetUserProfileAsync("non-existent-id");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetUserTransactionsAsync_WhenTransactionsExist_ReturnsTransactions()
        {
            // Arrange
            var userId = "test-user";
            var transactions = TestFaker.CreateSessionList();
            _mockUserRepository.Setup(r => r.GetUserTransactionsAsync(userId)).ReturnsAsync(transactions);

            // Act
            var result = await _sut.GetUserTransactionsAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(transactions.Count);
            result.Should().BeEquivalentTo(transactions);
        }

        [Fact]
        public async Task UpdateUserProfileSummaryAsync_WhenUserExists_UpdatesSummary()
        {
            // Arrange
            var userProfile = TestFaker.CreateUserProfile();
            var transactions = TestFaker.CreateSessionList();
            _mockUserRepository.Setup(r => r.GetUserProfileAsync(userProfile.Id)).ReturnsAsync(userProfile);
            _mockUserRepository.Setup(r => r.GetUserTransactionsAsync(userProfile.Id)).ReturnsAsync(transactions);

            // Act
            await _sut.UpdateUserProfileSummaryAsync(userProfile.Id);

            // Assert
            _mockUserRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
            userProfile.Summary.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetAllUserProfilesAsync_WhenCalled_ReturnsAllUserProfiles()
        {
            // Arrange
            var userProfiles = TestFaker.CreateUserProfileList();
            _mockUserRepository.Setup(r => r.GetAllUserProfilesAsync()).ReturnsAsync(userProfiles);

            // Act
            var result = await _sut.GetAllUserProfilesAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(userProfiles.Count);
        }

        [Fact]
        public async Task AddUserAsync_WithValidUser_ReturnsUser()
        {
            // Arrange
            var user = TestFaker.CreateUserProfile();
            _mockUserRepository.Setup(r => r.AddUserAsync(user)).ReturnsAsync(user);

            // Act
            var result = await _sut.AddUserAsync(user);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task UpdateUserAsync_WhenUserExists_UpdatesAndReturnsUser()
        {
            // Arrange
            var user = TestFaker.CreateUserProfile();
            _mockUserRepository.Setup(r => r.GetUserProfileAsync(user.Id)).ReturnsAsync(user);
            _mockUserRepository.Setup(r => r.UpdateUserAsync(user)).ReturnsAsync(user);

            // Act
            var result = await _sut.UpdateUserAsync(user);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task DeleteUserAsync_WhenUserExists_ReturnsTrue()
        {
            // Arrange
            var userId = "test-user";
            _mockUserRepository.Setup(r => r.DeleteUserAsync(userId)).ReturnsAsync(true);

            // Act
            var result = await _sut.DeleteUserAsync(userId);

            // Assert
            result.Should().BeTrue();
        }
    }

    public class TestFaker
    {
        private static readonly Faker _instance = new Faker();
        public static AutoFaker<UserProfile> UserProfileFaker => GetUserProfileFaker();

        public static AutoFaker<UserProfile> GetUserProfileFaker()
        {
            return (AutoFaker<UserProfile>)new AutoFaker<UserProfile>()
                .RuleFor(u => u.Id, f => f.Random.Guid().ToString())
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Summary, f => f.Lorem.Paragraph())
                .RuleFor(u => u.TotalSpent, f => f.Finance.Amount(0, 1000))
                .RuleFor(u => u.PurchaseCount, f => f.Random.Int(0, 100))
                .RuleFor(u => u.LastPurchase, f => f.Date.Past(1));
        }
        public static List<UserProfile> GetUserProfiles()
        {
            var faker = new Bogus.Faker<UserProfile>()
                .RuleFor(u => u.Id, f => f.Random.Guid().ToString())
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Summary, f => f.Lorem.Paragraph())
                .RuleFor(u => u.TotalSpent, f => f.Finance.Amount(0, 1000))
                .RuleFor(u => u.PurchaseCount, f => f.Random.Int(0, 100))
                .RuleFor(u => u.LastPurchase, f => f.Date.Past(1));

            return faker.Generate(10);
        }
    }
}
