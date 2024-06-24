namespace Fanex.Aff12B.Api.Core.Services
{
    using Constant;
    using Data.Repository;
    using Fanex.Aff12B.Common.Core.Helpers;
    using Models.Criteria;
    using Models.DTO;
    using Models.DTO.Statistics;
    using System;
    using System.Collections.Generic;

    public class AffService : IAffService
    {
        private readonly IDynamicRepository _repository;
        private readonly DateTime _currentDate;

        public AffService(IDynamicRepository repository)
        {
            _repository = repository;
            _currentDate = DateTimeHelper.Today;
        }

        public async Task<object> GetAccountOverviewForCurrentMonthAsync(int affId)
        {
            var accountOverviews = await _repository
                .FetchAsync<AccountOverview>(new AccountOverviewCriteria(affId, _currentDate))
                .ConfigureAwait(false);

            var totalAccountOverview = new
            {
                Signups = accountOverviews.Sum(i => i.Signups),
                DepositedMembers = accountOverviews.Sum(i => i.DepositedMembers),
                QualifiedActiveMembers = accountOverviews.Sum(i => i.QualifiedActiveMembers)
            };

            return totalAccountOverview;
        }

        public async Task<decimal> GetStatementHistoryForCurrentMonthAsync(int affId)
        {
            var statementHistories = await _repository
                .FetchAsync<StatementHistory>(new StatementHistoryCriteria { AffId = affId })
                .ConfigureAwait(false);

            var reportsForCurrentMonth = statementHistories
                .Where(s => s.MonthDate.Month == _currentDate.Month && s.MonthDate.Year == _currentDate.Year
                && s.TransType is TransType.MonthlyEarning or TransType.NewFDM)
                .Select(s => s.Amount);

            var total = reportsForCurrentMonth.Any() ? reportsForCurrentMonth.Sum() : 0;

            return total;
        }

        public async Task<decimal> GetNetWinLossForCurrentMonthAsync(int affId)
        {
            var reports = await _repository
                .FetchAsync<AffCommission>(new AffCommissionCriteria(affId, _currentDate))
                .ConfigureAwait(false);

            var totalReport = reports.FirstOrDefault(i => i.MonthDate.Month == _currentDate.Month
            && i.MonthDate.Year == _currentDate.Year && i.ProductId == 0);

            var netWinLoss = totalReport != null
                ? totalReport.WinLoss - totalReport.PlatformFee - totalReport.OperationalFee - totalReport.BankingFee - totalReport.Bonus
                : 0;

            return netWinLoss;
        }

        public async Task<IEnumerable<ReferrerStatistics>> GetReferrerStatisticsForCurrentMonthAsync(int affId)
        {
            var criteria = await CreateStatisticsCriteriaAsync<ReferrerStatisticsCriteria>(affId);
            return await _repository.FetchAsync<ReferrerStatistics>(criteria).ConfigureAwait(false);
        }

        public async Task<IEnumerable<LandingPageStatistics>> GetLandingPageStatisticsForCurrentMonthAsync(int affId)
        {
            var criteria = await CreateStatisticsCriteriaAsync<LandingPageStatisticsCriteria>(affId);
            return await _repository.FetchAsync<LandingPageStatistics>(criteria).ConfigureAwait(false);
        }

        private async Task<string> GetCountryCodesAsync()
        {
            var countries = await _repository
                .FetchAsync<Country>(new CountryCriteria())
                .ConfigureAwait(false);

            return string.Join(",", countries.Select(c => c.CountryCode));
        }

        private async Task<T> CreateStatisticsCriteriaAsync<T>(int affId) where T : new()
        {
            var fromDate = DateTimeHelper.CreateDateTime(_currentDate.Year, _currentDate.Month, 1);
            var countryCodes = await GetCountryCodesAsync();

            dynamic result = new T();

            result.AffId = affId;
            result.CountryCodes = countryCodes;
            result.FromDate = fromDate;
            result.ToDate = _currentDate;

            return (T)result;
        }
    }
}