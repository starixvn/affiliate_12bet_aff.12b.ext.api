namespace Fanex.Aff12B.Ext.Api.Core.Models.MetaModel
{
    public class StatementHistoryParam
    {
        public int AffId { get; set; }

        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 2000;

        public int NRows { get; set; }
    }
}
