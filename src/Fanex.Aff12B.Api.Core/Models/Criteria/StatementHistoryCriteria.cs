namespace Fanex.Aff12B.Api.Core.Models.Criteria
{
    using Data.Repository;

    public class StatementHistoryCriteria : CriteriaBase
    {
        public int AffId { get; set; }

        public int? PageIndex { get; set; }

        public int? PageSize { get; set; }

        public int NRows { get; set; }

        public override string GetSettingKey() => "Aff_User_StatementHistory_Sel";

        public override bool IsValid() => AffId > 0;
    }
}