namespace Fanex.Aff12B.Ext.Api.Core.Tests.Helpers
{
    using Helper;

    public class DateTimeHelperTest
    {
        [Fact]
        public void Now_WhenCalled_ShouldReturnCurrentDateTime()
        {
            // Arrange
            var currentDate = DateTimeOffset.Now;

            // Act
            var actual = DateTimeHelper.Now;

            // Assert
            Assert.Equal(currentDate.Date, actual.Date);
            Assert.Equal(currentDate.Hour, actual.Hour);
            Assert.Equal(currentDate.Minute, actual.Minute);
            Assert.Equal(currentDate.Second, actual.Second);
        }

        [Fact]
        public void UtcNow_WhenCalled_ShouldReturnCurrentUtcDateTime()
        {
            // Arrange
            var expectedUtcDateTime = DateTimeOffset.UtcNow;

            // Act
            var actualUtcDateTime = DateTimeHelper.UtcNow;

            // Assert
            Assert.Equal(expectedUtcDateTime.Date, actualUtcDateTime.Date);
            Assert.Equal(expectedUtcDateTime.Hour, actualUtcDateTime.Hour);
            Assert.Equal(expectedUtcDateTime.Minute, actualUtcDateTime.Minute);
            Assert.Equal(expectedUtcDateTime.Second, actualUtcDateTime.Second);
        }

        [Fact]
        public void Today_WhenCalled_ShouldReturnCurrentDate()
        {
            // Arrange
            var expectedDate = DateTimeOffset.Now.Date;

            // Act
            var actualDate = DateTimeHelper.Today;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void CurrentYear_WhenCalled_ShouldReturnCurrentYear()
        {
            // Arrange
            var expectedYear = DateTimeOffset.Now.Year;

            // Act
            var actualYear = DateTimeHelper.CurrentYear;

            // Assert
            Assert.Equal(expectedYear, actualYear);
        }

        [Fact]
        public void CreateDateTime_ShouldCreateDateTimeWithGivenParameters()
        {
            // Arrange
            const int year = 2024;
            const int month = 3;
            const int day = 27;

            var expectedDateTime = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.Zero).DateTime;

            // Act
            var actualDateTime = DateTimeHelper.CreateDateTime(year, month, day);

            // Assert
            Assert.Equal(expectedDateTime, actualDateTime);
        }
    }
}