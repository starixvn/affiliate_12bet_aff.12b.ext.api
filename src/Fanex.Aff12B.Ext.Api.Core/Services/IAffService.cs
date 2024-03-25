namespace Fanex.Aff12B.Ext.Api.Core.Services
{
    using Models.DTO.Statistics;

    public interface IAffService
    {
        Task<object> GetAccountOverviewForCurrentMonth(int affId);
        
        Task<decimal> GetStatementHistoryForCurrentMonth(int affId);

        Task<decimal> GetNetWinLossForCurrentMonth(int affId);

        Task<IEnumerable<ReferrerStatistics>> GetReferrerStatisticsForCurrentMonth(int affId);

        Task<IEnumerable<LandingPageStatistics>> GetLandingPageStatisticsForCurrentMonth(int affId);
    }
}