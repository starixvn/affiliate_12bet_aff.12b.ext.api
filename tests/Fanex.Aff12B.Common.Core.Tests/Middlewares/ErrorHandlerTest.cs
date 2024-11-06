using Fanex.Aff12B.Common.Core.Helpers;
using Fanex.Aff12B.Common.Core.Middlewares;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Net;

namespace Fanex.Aff12B.Common.Core.Tests.Middlewares
{
    public class ErrorHandlerTest
    {
        private RequestDelegate MockNext { get; set; }
        private ILogger<ErrorHandler> MockLogger { get; set; }
        private IErrorMessageHelper MockErrorHelper { get; set; }

        public ErrorHandlerTest()
        {
            MockNext = Substitute.For<RequestDelegate>();
            MockLogger = Substitute.For<ILogger<ErrorHandler>>();
            MockErrorHelper = Substitute.For<IErrorMessageHelper>();
            MockErrorHelper.GenerateMessage(Arg.Any<HttpContext>(), Arg.Any<Exception>()).Returns(string.Empty);
        }

        [Fact]
        public async Task Invoke_WhenCallWithNoException_NotLogging()
        {
            //Arrange
            var errorHandler = new ErrorHandler(MockNext, MockLogger, MockErrorHelper);

            //Action
            await errorHandler.Invoke(Substitute.For<HttpContext>());

            // Assert
            await MockNext.Received().Invoke(Arg.Any<HttpContext>());
        }

        [Fact]
        public async Task Invoke_WhenCallInvokeAndHasException_ErrorHandlerHandleExceptionAndCallNextInvoke()
        {
            //Arrange
            var context = Substitute.For<HttpContext>();
            var reponse = Substitute.For<HttpResponse>();
            var features = Substitute.For<IFeatureCollection>();

            using var stream = new MemoryStream();
            context.Response.ReturnsForAnyArgs(reponse);
            context.Features.ReturnsForAnyArgs(features);
            reponse.HttpContext.ReturnsForAnyArgs(context);
            reponse.Headers.ReturnsForAnyArgs(new HeaderDictionary());
            reponse.Body = stream;

            features[typeof(IExceptionHandlerFeature)].ReturnsForAnyArgs(new ExceptionHandlerFeature { Error = new Exception("Test") });
            var errorHandler = new ErrorHandler(MockNext, MockLogger, MockErrorHelper);

            //Action
            await errorHandler.Invoke(context);

            //Assert
            await MockNext.Received().Invoke(Arg.Any<HttpContext>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        }

        [Fact]
        public async Task Invoke_WhenCallNextInvokeAndThrowException_ErrorHandlerHandleException()
        {
            //Arrange
            var context = Substitute.For<HttpContext>();
            var reponse = Substitute.For<HttpResponse>();
            var features = Substitute.For<IFeatureCollection>();

            using var stream = new MemoryStream();
            context.Response.ReturnsForAnyArgs(reponse);
            context.Features.ReturnsForAnyArgs(features);
            reponse.HttpContext.ReturnsForAnyArgs(context);
            reponse.Headers.ReturnsForAnyArgs(new HeaderDictionary());
            reponse.Body = stream;

            features.Get<IHttpResponseFeature>().ReturnsForAnyArgs(new HttpResponseFeature());
            MockNext.Invoke(context).ReturnsForAnyArgs(x => throw new Exception("Test"));
            var errorHandler = new ErrorHandler(MockNext, MockLogger, MockErrorHelper);

            //Action
            await errorHandler.Invoke(context);

            //Assert
            await MockNext.Received().Invoke(Arg.Any<HttpContext>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        }
    }
}