using Fanex.Aff12B.Ext.Api.Core.Models.DTO;
using Fanex.Aff12B.Ext.Api.Core.Models.MetaModel;
using Fanex.Data;

namespace Fanex.Aff12B.Ext.Api.Core.Services
{
    public class AffService : IAffService
    {
        public IEnumerable<StatementHistory> GetStatementHistory(StatementHistoryParam param)
        {
            const string spName = "Aff_User_StatementHistory_Sel";

            using (var db = ObjectDbFactory.CreateInstance(spName))
            {
                return db.Query<StatementHistory>(param);
            }
        }
    }
}
