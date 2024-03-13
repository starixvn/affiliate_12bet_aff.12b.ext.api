using Fanex.Data;

namespace Fanex.Aff12B.Ext.Api.Core.Models.MetaModel
{
    public class AffCommissionParam
    {
        public int AffId { get; set; }

        [SpParam(Ignored = true)]
        public int Year { get; set; }

        [SpParam(Ignored = true)]
        public bool IsValid => Year >= 2009 && Year <= DateTimeOffset.Now.Year;

        public DateTime WinlostDate => new DateTime(Year, 1, 1);
    }
}
