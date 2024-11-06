using Microsoft.AspNetCore.Http;
using System.Text;

namespace Fanex.Aff12B.Common.Core.Extensions
{
    public static class StringBuilderExtensions
    {
        public static void AddRequestInfo(this StringBuilder builder, HttpContext context)
        {
            builder.AppendLine();
            builder.AppendLine("REQUEST INFO");

            var webInfo = GetRequestInfo(context);

            foreach (var item in webInfo)
            {
                builder.AppendLine($"{item.Key}: {item.Value}");
            }
        }

        private static Dictionary<string, object> GetRequestInfo(HttpContext context)
        {
            var request = context.Request;

            return new Dictionary<string, object>
            {
                [nameof(request.Host)] = request.Host,
                [nameof(request.Path)] = request.Path,
                [nameof(request.Body)] = request.Body,
                ["Remote IP"] = context.Connection.RemoteIpAddress,
                ["Client IP"] = context.Request.GetClientIP(),
            };
        }
    }
}
