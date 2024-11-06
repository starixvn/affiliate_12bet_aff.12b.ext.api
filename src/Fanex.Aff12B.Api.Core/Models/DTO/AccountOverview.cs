namespace Fanex.Aff12B.Api.Core.Models.DTO
{
    public class AccountOverview
    {
        public DateTime WinlostDate { get; set; }

        public int Signups { get; set; }

        public int DepositedMembers { get; set; }

        public int ActiveMembers { get; set; }

        public int QualifiedActiveMembers { get; set; }

        public decimal Deposit { get; set; }

        public decimal Withdrawal { get; set; }

        public decimal Turnover { get; set; }

        public decimal Winloss { get; set; }
    }
}