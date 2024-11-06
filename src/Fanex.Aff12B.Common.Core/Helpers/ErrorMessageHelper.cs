namespace Fanex.Aff12B.Common.Core.Helpers
{
    using Fanex.Aff12B.Common.Core.Utilities;
    using Microsoft.AspNetCore.Http;
    using System;

    public class ErrorMessageHelper : IErrorMessageHelper
    {
        public string GenerateMessage(HttpContext context, Exception exception)
        {
            return ExceptionUtils.GetExceptionMessagerBuilder(exception).ToString();
        }
    }
}
