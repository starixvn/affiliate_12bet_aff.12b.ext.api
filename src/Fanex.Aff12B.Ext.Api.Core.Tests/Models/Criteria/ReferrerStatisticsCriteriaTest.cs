namespace Fanex.Aff12B.Ext.Api.Core.Tests.Models.Criteria
{
    using Helper;
    using Fanex.Aff12B.Ext.Api.Core.Models.Criteria;

    public class ReferrerStatisticsCriteriaTest
    {
        private static readonly DateTime Now = DateTimeHelper.Today;

        [Fact]
        public void GetSettingKey_WhenCalled_ReturnCorrectSPKey()
        {
            // Arrange
            var criteria = new ReferrerStatisticsCriteria();

            // Action
            var key = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Aff_ClickStatisticsSelBySite", key);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void IsValid_ValidData_ReturnCorrect(int affId, DateTime fromDate, DateTime toDate, bool result)
        {
            // Arrange
            var criteria = new ReferrerStatisticsCriteria
            {
                AffId = affId,
                FromDate = fromDate,
                ToDate = toDate,
                DeviceType = 1,
                CountryCodes = "US,UK",
                TotalClicks = 0,
                UniqueClicks = 0,
                TotalSignUps = 0,
                TotalFdms = 0
            };

            // Assert
            Assert.Equal(result, criteria.IsValid());
        }

        public static IEnumerable<object[]> Data
            => new List<object[]>
            {
                new object[] { 1, Now, Now, true },
                new object[] { 0, Now, Now, false },
                new object[] { 1, Now, Now.AddDays(-1), false },
                new object[] { 0, Now, Now.AddDays(-1), false },
            };
    }
}
