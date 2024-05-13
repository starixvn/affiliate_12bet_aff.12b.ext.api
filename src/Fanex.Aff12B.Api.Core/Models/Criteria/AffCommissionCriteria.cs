namespace Fanex.Aff12B.Api.Core.Models.Criteria
{
    using Data.Repository;
    using Fanex.Aff12B.Api.Core.Constant;
    using Fanex.Aff12B.Common.Core.Helpers;

    public class AffCommissionCriteria : CriteriaBase
    {
        public AffCommissionCriteria(int affId, DateTime winlostDate)
        {
            AffId = affId;
            WinlostDate = winlostDate;
        }

        public int AffId { get; set; }

        public DateTime WinlostDate { get; set; }

        public override string GetSettingKey() => "Aff_User_Rpt_Commission";

        public override bool IsValid()
            => AffId > 0 && WinlostDate.Year >= LimitedDateTime.MinYear && WinlostDate.Year <= DateTimeHelper.CurrentYear;
    }
}