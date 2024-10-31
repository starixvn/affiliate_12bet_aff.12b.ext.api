using Fanex.Aff12B.Common.Core.Constants;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text;

namespace Fanex.Aff12B.Common.Core.Extensions
{
    public static class HttpRequestExtensions
    {
        private static readonly string HostName = Dns.GetHostName();

        public static bool FromInternal(this HttpRequest request)
            => HostName.Contains(request.Host.Host, StringComparison.OrdinalIgnoreCase);

        public static string GetClientIP(this HttpRequest request)
        {
            string clientIP = null;

            if (request.Headers.TryGetValue(HeaderKey.X_ACTUAL_IP, out var actualIP))
            {
                clientIP = actualIP[0];
            }

            if (string.IsNullOrWhiteSpace(clientIP) && request.Headers.TryGetValue(HeaderKey.X_FORWARDED_FOR, out var forwardedIP))
            {
                clientIP = forwardedIP[0];
            }

            return clientIP?.Trim() ?? request.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        public static void AddRequestInfo(HttpContext context, StringBuilder builder)
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
