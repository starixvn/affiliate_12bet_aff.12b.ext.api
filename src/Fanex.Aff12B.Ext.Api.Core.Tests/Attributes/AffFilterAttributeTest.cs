namespace Fanex.Aff12B.Ext.Api.Core.Tests.Attributes
{
    using Core.Attributes;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Configuration;

    public class AffFilterAttributeTest
    {
        [Fact]
        public void OnActionExecuting_ValidAccess_ResultIsNull()
        {
            // Arrange
            var myConfiguration = new Dictionary<string, string>
            {
                {"AffAccessList:0", "1"},
                {"AffAccessList:1", "2"},
                {"AffAccessList:3", "3"},
            };
            var configuration = FakeConfigurationRoot(myConfiguration);

            var actionExecutingContext = FakeActionExecutingContext();
            actionExecutingContext.ActionArguments["affId"] = 1;

            var filter = new AffFilterAttribute(configuration);

            // Action
            filter.OnActionExecuting(actionExecutingContext);

            // Assert
            Assert.Null(actionExecutingContext.Result);
        }

        [Fact]
        public void OnActionExecuting_WhenAffAccessListEmpty_ResultIsNull()
        {
            // Arrange
            var myConfiguration = new Dictionary<string, string>();
            var configuration = FakeConfigurationRoot(myConfiguration);

            var actionExecutingContext = FakeActionExecutingContext();

            var filter = new AffFilterAttribute(configuration);

            // Action
            filter.OnActionExecuting(actionExecutingContext);

            // Assert
            Assert.Null(actionExecutingContext.Result);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(null)]
        public void OnActionExecuting_InvalidParam_ReturnBadRequest(int? affId)
        {
            // Arrange
            var myConfiguration = new Dictionary<string, string>
            {
                {"AffAccessList:0", "1"},
                {"AffAccessList:1", "2"},
                {"AffAccessList:3", "3"},
            };
            var configuration = FakeConfigurationRoot(myConfiguration);

            var actionExecutingContext = FakeActionExecutingContext();
            actionExecutingContext.ActionArguments["affId"] = affId;

            var filter = new AffFilterAttribute(configuration);

            // Action
            filter.OnActionExecuting(actionExecutingContext);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actionExecutingContext.Result);
            var result = actionExecutingContext.Result as BadRequestObjectResult;
            Assert.Equal("API access is not permitted!", result?.Value);
        }

        private static IConfigurationRoot FakeConfigurationRoot(Dictionary<string, string> dic)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(dic!)
                .Build();

            return configuration;
        }

        private static ActionExecutingContext FakeActionExecutingContext()
        {
            var httpContext = new DefaultHttpContext();

            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor(), new ModelStateDictionary());

            var actionExecutingContext = new ActionExecutingContext(actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                controller: null!);

            return actionExecutingContext;
        }
    }
}