using Fanex.Aff12B.Ext.Api.Core.Models.DTO;
using Fanex.Aff12B.Ext.Api.Core.Models.MetaModel;
using Fanex.Data;

namespace Fanex.Aff12B.Ext.Api.Core.Services
{
    public class AffService : IAffService
    {
        public IEnumerable<AccountOverview> GetAccountOverview(AccountOverviewParam param)
        {
            const string spName = "Aff_User_Overview_Detail_Sel";

            using (var db = ObjectDbFactory.CreateInstance(spName))
            {
                return db.Query<AccountOverview>(param);
            }
        }

        public IEnumerable<StatementHistory> GetStatementHistory(StatementHistoryParam param)
        {
            const string spName = "Aff_User_StatementHistory_Sel";

            using (var db = ObjectDbFactory.CreateInstance(spName))
            {
                return db.Query<StatementHistory>(param);
            }
        }

        public IEnumerable<AffCommission> GetAffCommission(AffCommissionParam param)
        {
            const string spName = "Aff_User_Rpt_Commission";

            using (var db = ObjectDbFactory.CreateInstance(spName))
            {
                return db.Query<AffCommission>(param);
            }
        }
    }
}
