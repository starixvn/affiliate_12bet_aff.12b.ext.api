namespace Fanex.Aff12B.Ext.Api.Core.Tests.Services
{
    using Constant;
    using Core.Models.Criteria;
    using Core.Models.DTO;
    using Core.Models.DTO.Statistics;
    using Core.Services;
    using Data.Repository;
    using Helper;
    using NSubstitute;

    [Collection(nameof(AffServiceTest))]
    [CollectionDefinition(nameof(AffServiceTest), DisableParallelization = true)]
    public class AffServiceTest : IDisposable
    {
        private readonly IDynamicRepository _repository;
        private readonly AffService _affService;

        // Setup
        public AffServiceTest()
        {
            DateTimeHelper.DateTimeOffsetProvider = () => DateTimeHelper.CreateDateTime(2024, 2, 15);

            _repository = Substitute.For<IDynamicRepository>();
            _affService = new AffService(_repository);
        }

        // Teardown
        public void Dispose()
        {
            DateTimeHelper.DateTimeOffsetProvider = () => DateTimeOffset.Now;
        }

        [Fact]
        public async Task GetAccountOverviewForCurrentMonth_WhenCalled_ReturnCorrectData()
        {
            // Arrange
            const int affId = 123;
            var accountOverviews = new List<AccountOverview>
            {
                new() { WinlostDate = DateTimeHelper.Today, Signups = 10, DepositedMembers = 5, QualifiedActiveMembers = 3, ActiveMembers = 0, Deposit = 0, Withdrawal = 0, Turnover = 0, Winloss = 0 },
                new() { WinlostDate = DateTimeHelper.Today, Signups = 5, DepositedMembers = 2, QualifiedActiveMembers = 1, ActiveMembers = 0, Deposit = 0, Withdrawal = 0, Turnover = 0, Winloss = 0 }
            };

            _repository.FetchAsync<AccountOverview>(Arg.Any<AccountOverviewCriteria>()).Returns(accountOverviews);

            // Action
            var result = await _affService.GetAccountOverviewForCurrentMonth(affId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(15, result.GetType().GetProperty("Signups")?.GetValue(result, null));
            Assert.Equal(7, result.GetType().GetProperty("DepositedMembers")?.GetValue(result, null));
            Assert.Equal(4, result.GetType().GetProperty("QualifiedActiveMembers")?.GetValue(result, null));
        }

        [Fact]
        public async Task GetStatementHistoryForCurrentMonth_WhenCalled_ShouldReturnTotalAmountForCurrentMonth()
        {
            // Arrange
            const int affId = 123;
            var statementHistories = new List<StatementHistory>
            {
                new() { MonthDate = DateTimeHelper.CreateDateTime(2024, 2, 11), TransType = TransType.MonthlyEarning, Amount = 100, TransName = "Test", Status = "", Description = ""},
                new() { MonthDate = DateTimeHelper.CreateDateTime(2024, 2, 1), TransType = TransType.Adjustment, Amount = 50, TransName = "Test", Status = "", Description = "" },
                new() { MonthDate = DateTimeHelper.CreateDateTime(2024, 1, 24), TransType = TransType.NewFDM, Amount = 200, TransName = "Test", Status = "", Description = "" },
                new() {MonthDate = DateTimeHelper.CreateDateTime(2024, 2, 25), TransType = TransType.NewFDM, Amount = 300, TransName = "Test", Status = "", Description = ""}
            };
            _repository.FetchAsync<StatementHistory>(Arg.Any<StatementHistoryCriteria>()).Returns(statementHistories);

            // Action
            var totalAmount = await _affService.GetStatementHistoryForCurrentMonth(affId);

            // Assert
            Assert.Equal(400, totalAmount);
        }

        [Fact]
        public async Task GetStatementHistoryForCurrentMonth_WhenCalledWithIncorrectParam_ShouldReturnZero()
        {
            // Arrange
            const int affId = 0;

            // Action
            var totalAmount = await _affService.GetStatementHistoryForCurrentMonth(affId);

            // Assert
            Assert.Equal(0, totalAmount);
        }

        [Fact]
        public async Task GetNetWinLossForCurrentMonth_WhenCalled_ShouldReturnCorrectNetWinLossForCurrentMonth()
        {
            // Arrange
            const int affId = 123;
            var reports = new List<AffCommission>
            {
                new() { MonthDate = DateTimeHelper.CreateDateTime(2024, 2, 11), ProductId = 0, WinLoss = 1000, PlatformFee = 200, OperationalFee = 100, BankingFee = 50, Bonus = 150, ProductName = "", TurnOver = 0, CommissionRate = 0, CalculatedType = 0, Commission = 0, FdmCommission = 0 },
                new() { MonthDate = DateTimeHelper.CreateDateTime(2024, 2, 15), ProductId = 1, WinLoss = 1000, PlatformFee = 200, OperationalFee = 100, BankingFee = 50, Bonus = 150, ProductName = "", TurnOver = 0, CommissionRate = 0, CalculatedType = 0, Commission = 0, FdmCommission = 0 },
                new() { MonthDate = DateTimeHelper.CreateDateTime(2024, 1, 20), ProductId = 0, WinLoss = 1500, PlatformFee = 300, OperationalFee = 150, BankingFee = 75, Bonus = 200, ProductName = "", TurnOver = 0, CommissionRate = 0, CalculatedType = 0, Commission = 0, FdmCommission = 0 }
            };
            _repository.FetchAsync<AffCommission>(Arg.Any<AffCommissionCriteria>()).Returns(reports);

            // Action
            var netWinLoss = await _affService.GetNetWinLossForCurrentMonth(affId);

            // Assert
            Assert.Equal(500, netWinLoss);
        }

        [Fact]
        public async Task GetNetWinLossForCurrentMonth_WhenCalledIncorrectParams_ShouldReturnZero()
        {
            // Arrange
            const int affId = 0;

            // Action
            var netWinLoss = await _affService.GetNetWinLossForCurrentMonth(affId);

            // Assert
            Assert.Equal(0, netWinLoss);
        }

        [Fact]
        public async Task GetReferrerStatisticsForCurrentMonth_WhenCalled_ShouldReturnReferrerStatisticsForCurrentMonth()
        {
            // Arrange
            const int affId = 123;

            var referrerStatistics = new List<ReferrerStatistics>
            {
                new (){SiteUrls = "", Country = "", CountryCode = "", TotalClick = 0, UniqueClick = 0, SignUps = 0, Fdm = 0}
            };

            _repository.FetchAsync<Country>(Arg.Any<CountryCriteria>()).Returns(new List<Country> { new() { CountryCode = "Code", CountryName = "Name" } });
            _repository.FetchAsync<ReferrerStatistics>(Arg.Any<ReferrerStatisticsCriteria>()).Returns(referrerStatistics);

            // Action
            var result = await _affService.GetReferrerStatisticsForCurrentMonth(affId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetReferrerStatisticsForCurrentMonth_WhenCalledWithIncorrectParams_ShouldReturnEmpty()
        {
            // Arrange
            const int affId = 0;

            // Action
            var result = await _affService.GetReferrerStatisticsForCurrentMonth(affId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetLandingPageStatisticsForCurrentMonth_WhenCalled_ShouldReturnLandingPageStatisticsForCurrentMonth()
        {
            // Arrange
            const int affId = 123;

            var landingPageStatistics = new List<LandingPageStatistics>
            {
                new() { Page = "", Language = "", LanguageCode = "", Country = "", CountryCode = "" }
            };

            _repository.FetchAsync<Country>(Arg.Any<CountryCriteria>()).Returns(new List<Country>());
            _repository.FetchAsync<LandingPageStatistics>(Arg.Any<LandingPageStatisticsCriteria>()).Returns(landingPageStatistics);

            // Action
            var result = await _affService.GetLandingPageStatisticsForCurrentMonth(affId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetLandingPageStatisticsForCurrentMonth_WhenCalledWithIncorrectParams_ShouldReturnEmpty()
        {
            // Arrange
            const int affId = 0;

            // Action
            var result = await _affService.GetLandingPageStatisticsForCurrentMonth(affId);

            // Assert
            Assert.Empty(result);
        }
    }
}