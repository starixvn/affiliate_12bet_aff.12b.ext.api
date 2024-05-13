namespace Fanex.Aff12B.Api.Core.Models.Criteria
{
    using Data.Repository;

    public class LandingPageStatisticsCriteria : CriteriaBase
    {
        public int AffId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public byte DeviceType { get; set; }

        public string CountryCodes { get; set; }

        public int TotalSignUps { get; set; }

        public int TotalFdms { get; set; }

        public override string GetSettingKey() => "Aff_ClickStatisticsSelByPage";

        public override bool IsValid()
            => AffId > 0 && FromDate <= ToDate;
    }
}