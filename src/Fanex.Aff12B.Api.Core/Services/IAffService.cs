namespace Fanex.Aff12B.Api.Core.Services
{
    using Models.DTO.Statistics;

    public interface IAffService
    {
        Task<object> GetAccountOverviewForCurrentMonthAsync(int affId);

        Task<decimal> GetStatementHistoryForCurrentMonthAsync(int affId);

        Task<decimal> GetNetWinLossForCurrentMonthAsync(int affId);

        Task<IEnumerable<ReferrerStatistics>> GetReferrerStatisticsForCurrentMonthAsync(int affId);

        Task<IEnumerable<LandingPageStatistics>> GetLandingPageStatisticsForCurrentMonthAsync(int affId);
    }
}