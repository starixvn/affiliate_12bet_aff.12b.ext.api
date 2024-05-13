namespace Fanex.Aff12B.Common.Core.Helpers
{
    using Microsoft.AspNetCore.Http;
    using System;

    public interface IErrorMessageHelper
    {
        public string GenerateMessage(HttpContext context, Exception exception);
    }
}
