namespace Fanex.Aff12B.Common.Core.Helpers
{
    using Fanex.Aff12B.Common.Core.Extensions;
    using Fanex.Aff12B.Common.Core.Utilities;
    using Microsoft.AspNetCore.Http;
    using System;

    public class ErrorMessageHelper : IErrorMessageHelper
    {
        public string GenerateMessage(HttpContext context, Exception exception)
        {
            var messageBuilder = ExceptionUtils.GetExceptionMessagerBuilder(exception);
            messageBuilder.AddRequestInfo(context);

            return messageBuilder.ToString();
        }
    }
}
