namespace Fanex.Aff12B.Identity.Core.Tests.Services
{
    using Core.Extensions;
    using Core.Services;
    using Fanex.Aff12B.Common.Core.Models;
    using Microsoft.Extensions.Options;
    using NSubstitute;

    public class UserManagementServiceTest
    {
        private readonly List<User> users = [new User { Username = "test", Password = "1234".Sha256(), Role = string.Empty, Scopes = string.Empty }];
        private readonly IOptions<List<User>> _mockUsers;

        public UserManagementServiceTest()
        {
            _mockUsers = Substitute.For<IOptions<List<User>>>();
        }

        [Fact]
        public void GetValidUser_WhenCalled_GenerateHasedPassword()
        {
            //Arrange
            var rawUser = new User
            {
                Username = "Username",
                Password = "Password"
            };
            var hashedPassword = rawUser.Password.Sha256();
            _mockUsers.Value.Returns(new List<User> { new User { Username = rawUser.Username, Password = hashedPassword } });

            var userManagementService = new UserManagementService(_mockUsers);

            //Action
            var result = userManagementService.GetValidUser(rawUser.Username, rawUser.Password);

            //Assert
            Assert.NotNull(result);
        }


        [Theory]
        [InlineData("test", "123")]
        [InlineData(null, null)]
        [InlineData(null, "1234")]
        [InlineData("test", "")]
        public void GetValidUser_WhenCalled_ReturnCorrectResult(string userName, string pass)
        {
            //Arrange
            _mockUsers.Value.Returns(users);
            var userManagementService = new UserManagementService(_mockUsers);

            //Action
            var result = userManagementService.GetValidUser(userName, pass);

            //Assert
            Assert.Null(result);
        }
    }
}