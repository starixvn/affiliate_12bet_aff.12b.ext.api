namespace Fanex.Aff12B.Api.Core.Helpers
{
    using Fanex.Aff12B.Common.Core.Helpers;
    using Fanex.Aff12B.Common.Core.Utilities;
    using Fanex.Data;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;

    public class ErrorDbMessageHelper : IErrorMessageHelper
    {
        public string GenerateMessage(HttpContext context, Exception exception)
        {
            var messageBuilder = ExceptionUtils.GetExceptionMessagerBuilder(exception);
            AddRequestInfo(context, messageBuilder);
            AddDatabaseInfo(messageBuilder, exception);

            return messageBuilder.ToString();
        }

        private static void AddRequestInfo(HttpContext context, StringBuilder builder)
        {
            builder.AppendLine();
            builder.AppendLine("REQUEST INFO");

            var webInfo = GetRequestInfo(context);

            foreach (var item in webInfo)
            {
                builder.AppendLine($"{item.Key}: {item.Value}");
            }
        }

        private static void AddDatabaseInfo(StringBuilder builder, Exception exception)
        {
            builder.AppendLine();
            builder.AppendLine("DATABASE INFO");

            var dbInfo = GetDbExceptionInfo(exception);

            foreach (var item in dbInfo)
            {
                builder.AppendLine($"{item.Key}: {item.Value}");
            }
        }

        private static Dictionary<string, object> GetDbExceptionInfo(Exception exception)
        {
            var dbInfo = new Dictionary<string, object>();
            BindSqlExceptionInfo(exception, dbInfo);
            BindFanexDataExceptionInfo(exception, dbInfo);
            return dbInfo;
        }

        private static void BindFanexDataExceptionInfo(Exception exception, IDictionary<string, object> dbInfo)
        {
            if (exception is ObjectDbException fanexDataException)
            {
                BindSqlExceptionInfo(exception.InnerException, dbInfo);

                var connString = new SqlConnectionStringBuilder(fanexDataException.ConnectionString);

                dbInfo["Server"] = connString.DataSource;
                dbInfo["DbName"] = fanexDataException.DbName;
                dbInfo["SpName"] = fanexDataException.SpName;
                dbInfo["UserId"] = connString.UserID;
                dbInfo["Parameters"] = GetParameters(fanexDataException.Parameters);
                dbInfo["CommandTimeout"] = fanexDataException.CommandTimeout;
            }
        }

        private static void BindSqlExceptionInfo(Exception exception, IDictionary<string, object> dbInfo)
        {
            if (exception is SqlException sqlException)
            {
                dbInfo["Server"] = sqlException.Server;
                dbInfo["SpName"] = sqlException.Procedure;
                dbInfo["Line"] = sqlException.LineNumber;
            }
        }

        private static string GetParameters(IDictionary<string, object> parameters)
           => parameters?.Count > 0
               ? parameters.Select(parameter => $"{parameter.Key}={parameter.Value}")
                       .Aggregate((firstPair, secondPair) => $"{firstPair};{secondPair}")
               : string.Empty;

        private static Dictionary<string, object> GetRequestInfo(HttpContext context)
        {
            var request = context.Request;

            return new Dictionary<string, object>
            {
                [nameof(request.Host)] = request.Host,
                [nameof(request.Path)] = request.Path,
                [nameof(request.Body)] = request.Body,
                ["Remote IP"] = context.Connection.RemoteIpAddress
            };
        }
    }
}
