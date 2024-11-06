namespace Fanex.Aff12B.Api.Core.Tests.Models.Criteria
{
    using Fanex.Aff12B.Api.Core.Models.Criteria;
    using Fanex.Aff12B.Common.Core.Helpers;

    public class AffCommissionCriteriaTest
    {
        private static readonly DateTime Now = DateTimeHelper.Today;

        [Fact]
        public void GetSettingKey_WhenCalled_ReturnCorrectSPKey()
        {
            // Arrange
            var criteria = new AffCommissionCriteria(1, Now);

            // Action
            var key = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Aff_User_Rpt_Commission", key);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void IsValid_ValidData_ReturnCorrect(int affId, DateTime date, bool result)
        {
            // Arrange
            var criteria = new AffCommissionCriteria(affId, date);

            // Assert
            Assert.Equal(result, criteria.IsValid());
        }

        public static IEnumerable<object[]> Data
            => new List<object[]>
            {
                new object[] { 1, Now, true },
                new object[] { 0, Now, false },
                new object[] { 1, DateTimeHelper.CreateDateTime(2008, 1, 1), false },
                new object[] { 1, Now.AddYears(1), false },
                new object[] { 0,  DateTimeHelper.CreateDateTime(2008, 1, 1), false},
            };
    }
}
