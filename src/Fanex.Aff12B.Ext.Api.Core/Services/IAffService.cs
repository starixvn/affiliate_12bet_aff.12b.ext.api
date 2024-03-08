using Fanex.Aff12B.Ext.Api.Core.Models.DTO;
using Fanex.Aff12B.Ext.Api.Core.Models.MetaModel;

namespace Fanex.Aff12B.Ext.Api.Core.Services
{
    public interface IAffService
    {
        public IEnumerable<AccountOverview> GetAccountOverview(AccountOverviewParam param);

        public IEnumerable<StatementHistory> GetStatementHistory(StatementHistoryParam param);

        public IEnumerable<AffCommission> GetAffCommission(AffCommissionParam param);
    }
}
