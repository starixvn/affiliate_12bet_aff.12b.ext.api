namespace Fanex.Aff12B.Identity.Core.Tests.Services
{
    using Common.Core.Models;
    using Core.Extensions;
    using Core.Models.MetaModel;
    using Core.Services;
    using Microsoft.Extensions.Options;
    using NSubstitute;

    public class TokenAuthenticationServiceTest
    {
        private readonly IUserManagementService _mockUserManagement;
        private readonly TokenAuthenticationService _authenticateService;
        private readonly TokenManagementOptions _tokenManagement = new TokenManagementOptions
        {
            AccessExpiration = 30,
            Audience = "test",
            Issuer = "test",
            Secret = "04241d01a776a7e5095b7dffbff6518f33c9e325c6f6cd72b2d1c59b94755d16"
        };

        public TokenAuthenticationServiceTest()
        {
            _mockUserManagement = Substitute.For<IUserManagementService>();
            var mockTokenManagement = Substitute.For<IOptions<TokenManagementOptions>>();
            mockTokenManagement.Value.Returns(_tokenManagement);

            _authenticateService = new TokenAuthenticationService(
                _mockUserManagement,
               mockTokenManagement);
        }

        [Theory]
        [InlineData("asx", "1234aa")]
        [InlineData("12bet@asx", "1234aa")]
        public void IsAuthenticated_InValidUser_ReturnFalse(string userName, string pass)
        {
            //Arrange
            var token = new TokenRequestParam
            {
                Username = userName,
                Password = pass
            };
            _mockUserManagement.GetValidUser(token.Username, token.Password).Returns(null as User);

            //Action
            var result = _authenticateService.IsAuthenticated(token);

            //Assert
            Assert.False(result.Item1);
        }

        [Theory]
        [InlineData("asx", "1234aa")]
        [InlineData("12bet@asx", "1234aa")]
        public void IsAuthenticated_ValidUser_ReturnTrueAndNotNullToken(string userName, string pass)
        {
            //Arrange
            var token = new TokenRequestParam
            {
                Username = userName,
                Password = pass
            };
            _mockUserManagement.GetValidUser(Arg.Any<string>(), token.Password).Returns(new User { Username = userName, Password = pass.Sha256(), Role = string.Empty, Scopes = string.Empty });

            //Action
            var result = _authenticateService.IsAuthenticated(token);

            //Assert
            Assert.True(result.Item1);
            Assert.NotNull(result.Item2);
            Assert.Equal(30, result.Item2.ExpiresIn);
            Assert.False(string.IsNullOrEmpty(result.Item2.TokenType));
        }
    }
}