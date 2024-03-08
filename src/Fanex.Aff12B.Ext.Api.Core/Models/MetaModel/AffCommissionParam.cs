
namespace Fanex.Aff12B.Ext.Api.Core.Models.MetaModel
{
    public class AffCommissionParam
    {
        public int AffId { get; set; }

        public int Year { get; set; }

        public bool IsValid => Year >= 2009 && Year <= DateTimeOffset.Now.Year;
    }
}
