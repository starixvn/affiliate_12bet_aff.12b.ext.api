namespace Fanex.Aff12B.Ext.Api.Core.Models.Criteria
{
    using Data.Repository;
    using Fanex.Aff12B.Ext.Api.Core.Constant;
    using Helper;

    public class AccountOverviewCriteria : CriteriaBase
    {
        public AccountOverviewCriteria(int affId, DateTime monthDate)
        {
            AffId = affId;
            MonthDate = monthDate;
        }

        public int AffId { get; set; }

        public DateTime MonthDate { get; set; }

        public override string GetSettingKey() => "Aff_User_Overview_Detail_Sel";

        public override bool IsValid()
            => AffId > 0 && MonthDate >= DateTimeHelper.CreateDateTime(LimitedDateTime.MinYear, 1, 1) && MonthDate <= DateTimeHelper.Today;
    }
}