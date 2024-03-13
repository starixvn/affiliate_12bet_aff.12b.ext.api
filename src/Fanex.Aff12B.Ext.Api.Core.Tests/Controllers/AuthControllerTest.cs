using System.Net;
using Fanex.Aff12B.Ext.Api.Core.Controllers;
using Fanex.Aff12B.Ext.Api.Core.Models.DTO;
using Fanex.Aff12B.Ext.Api.Core.Models.MetaModel;
using Fanex.Aff12B.Ext.Api.Core.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace Fanex.Aff12B.Ext.Api.Core.Tests.Controllers
{
    public class AuthControllerTest
    {
        private readonly IAuthenticateService _authenticateService;

        public AuthControllerTest()
        {
            _authenticateService = Substitute.For<IAuthenticateService>();
        }

        [Fact]
        public void RequestToken_WhenCallWith_InvalidModel_ReturnBadRequest()
        {
            //Arrange
            var controller = new AuthController(_authenticateService);
            controller.ModelState.AddModelError("Error1", "Invalid");

            //Action
            var result = controller.RequestToken(new TokenRequestParam()) as ObjectResult;

            //Assert
            Assert.Equal("Invalid Request", result?.Value);
            Assert.Equal((int)HttpStatusCode.BadRequest, result?.StatusCode);
        }

        [Fact]
        public void RequestToken_WhenCallWith_Unauthentication_ReturnCorrect()
        {
            //Arrange
            _authenticateService.IsAuthenticated(Arg.Any<TokenRequestParam>()).Returns(Tuple.Create<bool, Token>(false, new Token()));
            var controller = new AuthController(_authenticateService);

            //Action
            var result = controller.RequestToken(new TokenRequestParam()) as ObjectResult;

            //Assert
            Assert.Equal("Wrong username or password", result?.Value);
            Assert.Equal((int)HttpStatusCode.BadRequest, result?.StatusCode);
        }

        [Fact]
        public void RequestToken_WhenCallWith_Authenticated_ReturnCorrect()
        {
            //Arrange
            _authenticateService.IsAuthenticated(Arg.Any<TokenRequestParam>()).Returns(Tuple.Create<bool, Token>(true, new Token()));
            var controller = new AuthController(_authenticateService);

            //Action
            var result = controller.RequestToken(new TokenRequestParam()) as ObjectResult;

            //Assert
            Assert.NotNull(result?.Value as Token);
            Assert.Equal((int)HttpStatusCode.OK, result?.StatusCode);
        }
    }
}