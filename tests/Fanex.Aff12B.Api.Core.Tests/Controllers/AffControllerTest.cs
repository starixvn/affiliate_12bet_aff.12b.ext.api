namespace Fanex.Aff12B.Api.Core.Tests.Controllers
{
    using Core.Controllers;
    using Core.Models.DTO.Statistics;
    using Core.Services;
    using Microsoft.AspNetCore.Mvc;
    using NSubstitute;

    public class AffControllerTest
    {
        private readonly IAffService _affService;
        private readonly AffController _affController;

        // Setup
        public AffControllerTest()
        {
            _affService = Substitute.For<IAffService>();
            _affController = new AffController(_affService);
        }

        [Fact]
        public async Task AccountOverviewAsync_WhenCalled_ReturnsOkWithCorrectData()
        {
            // Arrange
            const int affId = 123;
            var expectedData = new { Signups = 10, DepositedMembers = 5, QualifiedActiveMembers = 3 };

            _affService.GetAccountOverviewForCurrentMonthAsync(Arg.Any<int>()).Returns(expectedData);

            // Action
            var result = await _affController.AccountOverviewAsync(affId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedData, okResult.Value);
        }

        [Fact]
        public async Task NetWinLossAsync_WhenCalled_ReturnsOkWithCorrectData()
        {
            // Arrange
            const int affId = 123;
            const decimal expected = 100;
            _affService.GetNetWinLossForCurrentMonthAsync(Arg.Any<int>()).Returns(expected);

            // Action
            var result = await _affController.NetWinLossAsync(affId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(expected, okResult.Value);
        }

        [Fact]
        public async Task StatementHistoryAsync_WhenCalled_ReturnsOkWithCorrectData()
        {
            // Arrange
            const int affId = 123;
            const decimal expected = 100;
            _affService.GetStatementHistoryForCurrentMonthAsync(Arg.Any<int>()).Returns(expected);

            // Action
            var result = await _affController.StatementHistoryAsync(affId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(expected, okResult.Value);
        }

        [Fact]
        public async Task ReferrersStatisticsAsync_WhenCalled_ReturnsOkWithCorrectData()
        {
            // Arrange
            const int affId = 123;
            var expected = new List<ReferrerStatistics>();

            _affService.GetReferrerStatisticsForCurrentMonthAsync(Arg.Any<int>()).Returns(expected);

            // Action
            var result = await _affController.ReferrersStatisticsAsync(affId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(expected, okResult.Value);
        }

        [Fact]
        public async Task LandingPageStatisticsAsync_WhenCalled_ReturnsOkWithCorrectData()
        {
            // Arrange
            const int affId = 123;
            var expected = new List<LandingPageStatistics>();

            _affService.GetLandingPageStatisticsForCurrentMonthAsync(Arg.Any<int>()).Returns(expected);

            // Action
            var result = await _affController.LandingPageStatisticsAsync(affId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(expected, okResult.Value);
        }
    }
}
