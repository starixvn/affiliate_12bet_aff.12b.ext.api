namespace Fanex.Aff12B.Api.Core.Tests.Services
{
    using Common.Core.Helpers;
    using Constant;
    using Core.Models.Criteria;
    using Core.Models.DTO;
    using Core.Models.DTO.Statistics;
    using Core.Services;
    using Data.Repository;
    using NSubstitute;

    [Collection(nameof(AffServiceTest))]
    [CollectionDefinition(nameof(AffServiceTest), DisableParallelization = true)]
    public class AffServiceTest : IDisposable
    {
        private readonly IDynamicRepository _repository;
        private readonly AffService _affService;
        private bool disposedValue;

        // Setup
        public AffServiceTest()
        {
            DateTimeHelper.DateTimeOffsetProvider = () => DateTimeHelper.CreateDateTime(2024, 2, 15);

            _repository = Substitute.For<IDynamicRepository>();
            _affService = new AffService(_repository);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task GetAccountOverviewForCurrentMonthAsync_WhenCalled_ReturnCorrectData()
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
            var result = await _affService.GetAccountOverviewForCurrentMonthAsync(affId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(15, result.GetType().GetProperty("Signups")?.GetValue(result, null));
            Assert.Equal(7, result.GetType().GetProperty("DepositedMembers")?.GetValue(result, null));
            Assert.Equal(4, result.GetType().GetProperty("QualifiedActiveMembers")?.GetValue(result, null));
        }

        [Fact]
        public async Task GetStatementHistoryForCurrentMonthAsync_WhenCalled_ShouldReturnTotalAmountForCurrentMonth()
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
            var totalAmount = await _affService.GetStatementHistoryForCurrentMonthAsync(affId);

            // Assert
            Assert.Equal(400, totalAmount);
        }

        [Fact]
        public async Task GetStatementHistoryForCurrentMonthAsync_WhenCalledWithIncorrectParam_ShouldReturnZero()
        {
            // Arrange
            const int affId = 0;

            // Action
            var totalAmount = await _affService.GetStatementHistoryForCurrentMonthAsync(affId);

            // Assert
            Assert.Equal(0, totalAmount);
        }

        [Fact]
        public async Task GetNetWinLossForCurrentMonthAsync_WhenCalled_ShouldReturnCorrectNetWinLossForCurrentMonth()
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
            var netWinLoss = await _affService.GetNetWinLossForCurrentMonthAsync(affId);

            // Assert
            Assert.Equal(500, netWinLoss);
        }

        [Fact]
        public async Task GetNetWinLossForCurrentMonthAsync_WhenCalledIncorrectParams_ShouldReturnZero()
        {
            // Arrange
            const int affId = 0;

            // Action
            var netWinLoss = await _affService.GetNetWinLossForCurrentMonthAsync(affId);

            // Assert
            Assert.Equal(0, netWinLoss);
        }

        [Fact]
        public async Task GetReferrerStatisticsForCurrentMonthAsync_WhenCalled_ShouldReturnReferrerStatisticsForCurrentMonth()
        {
            // Arrange
            const int affId = 123;

            var referrerStatistics = new List<ReferrerStatistics>
            {
                new (){SiteUrls = "", Country = "", CountryCode = "", TotalClick = 0, UniqueClick = 0, SignUps = 0, Fdm = 0}
            };

            _repository.FetchAsync<Country>(Arg.Any<CountryCriteria>()).Returns([new() { CountryCode = "Code", CountryName = "Name" }]);
            _repository.FetchAsync<ReferrerStatistics>(Arg.Any<ReferrerStatisticsCriteria>()).Returns(referrerStatistics);

            // Action
            var result = await _affService.GetReferrerStatisticsForCurrentMonthAsync(affId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetReferrerStatisticsForCurrentMonthAsync_WhenCalledWithIncorrectParams_ShouldReturnEmpty()
        {
            // Arrange
            const int affId = 0;

            // Action
            var result = await _affService.GetReferrerStatisticsForCurrentMonthAsync(affId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetLandingPageStatisticsForCurrentMonthAsync_WhenCalled_ShouldReturnLandingPageStatisticsForCurrentMonth()
        {
            // Arrange
            const int affId = 123;

            var landingPageStatistics = new List<LandingPageStatistics>
            {
                new() { Page = "", Country = "", CountryCode = "" }
            };

            _repository.FetchAsync<Country>(Arg.Any<CountryCriteria>()).Returns([]);
            _repository.FetchAsync<LandingPageStatistics>(Arg.Any<LandingPageStatisticsCriteria>()).Returns(landingPageStatistics);

            // Action
            var result = await _affService.GetLandingPageStatisticsForCurrentMonthAsync(affId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetLandingPageStatisticsForCurrentMonthAsync_WhenCalledWithIncorrectParams_ShouldReturnEmpty()
        {
            // Arrange
            const int affId = 0;

            // Action
            var result = await _affService.GetLandingPageStatisticsForCurrentMonthAsync(affId);

            // Assert
            Assert.Empty(result);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~AffServiceTest()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }
    }
}