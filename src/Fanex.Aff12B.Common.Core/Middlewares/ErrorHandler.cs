using Fanex.Aff12B.Common.Core.Helpers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Fanex.Aff12B.Common.Core.Middlewares
{
    public class ErrorHandler(RequestDelegate next, ILogger<ErrorHandler> logger, IErrorMessageHelper helper)
    {
        private const int ServerErrorCode = 500;

        public async Task Invoke(HttpContext context)
        {
            if (context.Features[typeof(IExceptionHandlerFeature)] is IExceptionHandlerFeature error && error.Error != null)
            {
                await HandleError(context, error.Error);
            }

            try
            {
                await next.Invoke(context);
            }
            catch (Exception exception)
            {
                await HandleError(context, exception);
            }
        }

        private async Task HandleError(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = ServerErrorCode;
            var errorMsg = helper.GenerateMessage(context, ex);
            logger.LogCritical(ex, errorMsg);

            await context.Response.WriteAsJsonAsync(JsonConvert.SerializeObject(new
            {
                State = "Internal Server Error",
                Msg = "Sorry, an error occurred while processing your request."
            }));
        }
    }
}
