namespace Fanex.Aff12B.Ext.Api.Core.Models.Criteria
{
    using Data.Repository;

    public class ReferrerStatisticsCriteria : CriteriaBase
    {
        public int AffId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public byte DeviceType { get; set; }

        public int TotalClicks { get; set; }

        public int UniqueClicks { get; set; }

        public int TotalSignUps { get; set; }

        public int TotalFdms { get; set; }

        public string CountryCodes { get; set; }

        public override string GetSettingKey() => "Aff_ClickStatisticsSelBySite";

        public override bool IsValid()
            => AffId > 0 && FromDate <= ToDate;
    }
}