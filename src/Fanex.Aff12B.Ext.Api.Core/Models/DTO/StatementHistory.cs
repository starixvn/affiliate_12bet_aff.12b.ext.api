namespace Fanex.Aff12B.Ext.Api.Core.Models.DTO
{
    [Serializable]
    public class StatementHistory
    {
        public DateTime MonthDate { get; set; }

        public int TransType { get; set; }

        public string TransName { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; }

        public string Description { get; set; }
    }
}
