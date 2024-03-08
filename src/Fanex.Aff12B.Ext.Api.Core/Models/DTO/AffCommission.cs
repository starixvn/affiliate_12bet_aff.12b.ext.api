
namespace Fanex.Aff12B.Ext.Api.Core.Models.DTO
{
    public class AffCommission
    {
        public DateTime MonthDate { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal TurnOver { get; set; }

        public decimal WinLoss { get; set; }

        public decimal PlatformFee { get; set; }

        public decimal OperationalFee { get; set; }

        public decimal BankingFee { get; set; }

        public decimal Bonus { get; set; }

        public decimal CommissionRate { get; set; }

        public decimal Commission { get; set; }

        public int CalculatedType { get; set; }

        public decimal FdmCommission { get; set; }

        public decimal TotalCommission => Commission + FdmCommission;
    }
}
