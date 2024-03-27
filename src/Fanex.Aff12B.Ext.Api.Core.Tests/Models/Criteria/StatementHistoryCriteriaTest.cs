namespace Fanex.Aff12B.Ext.Api.Core.Tests.Models.Criteria
{
    using Fanex.Aff12B.Ext.Api.Core.Models.Criteria;

    public class StatementHistoryCriteriaTest
    {
        [Fact]
        public void GetSettingKey_WhenCalled_ReturnCorrectSPKey()
        {
            // Arrange
            var criteria = new StatementHistoryCriteria { AffId = 1 };

            // Action
            var key = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Aff_User_StatementHistory_Sel", key);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(0, false)]
        public void IsValid_ValidData_ReturnCorrect(int affId, bool result)
        {
            // Arrange
            var criteria = new StatementHistoryCriteria
            {
                AffId = affId,
                PageIndex = 1,
                PageSize = 10,
                NRows = 0
            };

            // Assert
            Assert.Equal(result, criteria.IsValid());
        }
    }
}