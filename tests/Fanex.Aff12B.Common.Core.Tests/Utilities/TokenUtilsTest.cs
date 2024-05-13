namespace Fanex.Aff12B.Common.Core.Tests.Utilities
{
    using Fanex.Aff12B.Common.Core.Utilities;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    public class TokenUtilsTest
    {
        [Fact]
        public void GetTokenValidationParameters_TokenIsNull_ReturnDefault()
        {
            // Arrange
            var myConfig = new Dictionary<string, string>();
            var configuration = FakeConfigurationRoot(myConfig);

            // Action
            var result = TokenUtils.GetTokenValidationParameters(configuration);

            // Assert
            Assert.IsType<TokenValidationParameters>(result);
        }

        [Fact]
        public void GetTokenValidationParameters_ValidToken_ReturnCorrectData()
        {
            // Arrange
            var myConfig = new Dictionary<string, string>
            {
                {"TokenManagement:Secret", "secret"},
                {"TokenManagement:Issuer", "12B"},
                {"TokenManagement:Audience", "12B"},
                {"TokenManagement:AccessExpiration", "30"},
                {"TokenManagement:RefreshExpiration", "60"}
            };
            var configuration = FakeConfigurationRoot(myConfig);

            // Action
            var result = TokenUtils.GetTokenValidationParameters(configuration);

            // Assert
            Assert.IsType<TokenValidationParameters>(result);
            Assert.Equal("12B", result.ValidIssuer);
            Assert.Equal("12B", result.ValidAudience);
        }

        private static IConfigurationRoot FakeConfigurationRoot(Dictionary<string, string> dic)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(dic!)
                .Build();

            return configuration;
        }
    }
}
