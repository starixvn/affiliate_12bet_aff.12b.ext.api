namespace Fanex.Aff12B.Api.Core.Models.Criteria
{
    using Data.Repository;

    public class CountryCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Aff_Country_Sel";

        public override bool IsValid() => true;
    }
}