
using Fanex.Data;

namespace Fanex.Aff12B.Ext.Api.Core.Models.MetaModel
{
    public class AccountOverviewParam
    {
        public int AffId { get; set; }

        public DateTime MonthDate { get; set; }

        [SpParam(Ignored = true)]
        public bool IsValid => MonthDate >= new DateTime(2009, 1, 1) && MonthDate <= DateTime.Now;
    }
}
