using Fanex.Aff12B.Common.Core.Constants;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Fanex.Aff12B.Common.Core.Extensions
{
    public static class HttpRequestExtensions
    {
        private const string IPv4Regex = "^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";
        private static readonly string HostName = Dns.GetHostName();

        public static bool FromInternal(this HttpRequest request)
            => HostName.Contains(request.Host.Host, StringComparison.OrdinalIgnoreCase);

        public static string GetClientIP(this HttpRequest request, string fakeForwarded = null)
        {
            string clientIPs = null;
            var remoteIpAddress = request.HttpContext.Connection.RemoteIpAddress.ToString();

            if (!string.IsNullOrEmpty(fakeForwarded))
            {
                return GetValidOrRemoteIpAddress(fakeForwarded, remoteIpAddress);
            }

            if (request.Headers.TryGetValue(HeaderKey.X_ACTUAL_IP, out var actualIPs))
            {
                clientIPs = actualIPs;
            }

            if (string.IsNullOrWhiteSpace(clientIPs) && request.Headers.TryGetValue(HeaderKey.X_FORWARDED_FOR, out var forwardedIPs))
            {
                clientIPs = forwardedIPs;
            }

            return GetValidOrRemoteIpAddress(clientIPs, remoteIpAddress);
        }

        private static string GetValidOrRemoteIpAddress(string ips, string remoteIpAddress)
        {
            var validIp = GetValidIPAddress(ips);
            if (string.IsNullOrWhiteSpace(validIp))
            {
                validIp = remoteIpAddress;
            }

            return validIp.Equals("::1") ? "127.0.0.1" : validIp;
        }

        private static string GetValidIPAddress(string ips)
        {
            if (string.IsNullOrEmpty(ips))
            {
                return null;
            }

            var ipList = ips.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim())
                        .Where(i => Regex.IsMatch(i, IPv4Regex, RegexOptions.None, TimeSpan.FromSeconds(2)));

            return ipList.FirstOrDefault();
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
