namespace Fanex.Aff12B.Ext.Api.Core.Tests.Models.Criteria
{
    using Fanex.Aff12B.Ext.Api.Core.Models.Criteria;

    public class CountryCriteriaTest
    {
        [Fact]
        public void GetSettingKey_WhenCalled_ReturnCorrectSPKey()
        {
            // Arrange
            var criteria = new CountryCriteria();

            // Action
            var key = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Aff_Country_Sel", key);
        }

        [Fact]
        public void IsValid_ValidData_ReturnCorrect()
        {
            // Arrange
            var criteria = new CountryCriteria();

            // Assert
            Assert.True(criteria.IsValid());
        }
    }
}
