using Fanex.Aff12B.Ext.Api.Core.Helper;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Fanex.Aff12B.Ext.Api.Core.Middleware
{
    public class ErrorHandler(RequestDelegate next, ILogger<ErrorHandler> logger)
    {
        private const int ServerErrorCode = 500;

        public async Task Invoke(HttpContext context)
        {
            var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;

            if (error != null && error.Error != null)
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

        private async Task HandleError(HttpContext context, Exception error)
        {
            context.Response.StatusCode = ServerErrorCode;
            var errorMsg = ErrorMessageHelper.GenerateMessage(context, error);
            logger.LogCritical(error, errorMsg);
            await context.Response.WriteAsJsonAsync(JsonConvert.SerializeObject(new
            {
                State = "Internal Server Error",
                Msg = "Sorry, an error occurred while processing your request."
            }));
        }
    }
}
