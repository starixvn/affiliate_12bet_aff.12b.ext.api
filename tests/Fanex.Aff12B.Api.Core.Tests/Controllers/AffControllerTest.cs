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
        public async Task AccountOverview_WhenCalled_ReturnsOkWithCorrectData()
        {
            // Arrange
            const int affId = 123;
            var expectedData = new { Signups = 10, DepositedMembers = 5, QualifiedActiveMembers = 3 };

            _affService.GetAccountOverviewForCurrentMonth(Arg.Any<int>()).Returns(expectedData);

            // Action
            var result = await _affController.AccountOverview(affId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedData, okResult.Value);
        }

        [Fact]
        public async Task NetWinLoss_WhenCalled_ReturnsOkWithCorrectData()
        {
            // Arrange
            const int affId = 123;
            const decimal expected = 100;
            _affService.GetNetWinLossForCurrentMonth(Arg.Any<int>()).Returns(expected);

            // Action
            var result = await _affController.NetWinLoss(affId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(expected, okResult.Value);
        }

        [Fact]
        public async Task StatementHistory_WhenCalled_ReturnsOkWithCorrectData()
        {
            // Arrange
            const int affId = 123;
            const decimal expected = 100;
            _affService.GetStatementHistoryForCurrentMonth(Arg.Any<int>()).Returns(expected);

            // Action
            var result = await _affController.StatementHistory(affId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(expected, okResult.Value);
        }

        [Fact]
        public async Task ReferrersStatistics_WhenCalled_ReturnsOkWithCorrectData()
        {
            // Arrange
            const int affId = 123;
            var expected = new List<ReferrerStatistics>();

            _affService.GetReferrerStatisticsForCurrentMonth(Arg.Any<int>()).Returns(expected);

            // Action
            var result = await _affController.ReferrersStatistics(affId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(expected, okResult.Value);
        }

        [Fact]
        public async Task LandingPageStatistics_WhenCalled_ReturnsOkWithCorrectData()
        {
            // Arrange
            const int affId = 123;
            var expected = new List<LandingPageStatistics>();

            _affService.GetLandingPageStatisticsForCurrentMonth(Arg.Any<int>()).Returns(expected);

            // Action
            var result = await _affController.LandingPageStatistics(affId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(expected, okResult.Value);
        }
    }
}
