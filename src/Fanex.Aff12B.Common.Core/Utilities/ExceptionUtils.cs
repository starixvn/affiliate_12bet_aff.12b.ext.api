using System.Text;

namespace Fanex.Aff12B.Common.Core.Utilities
{
    public static class ExceptionUtils
    {
        private const int LIMITED_INNER_EXCEPTION = 3;

        public static StringBuilder GetExceptionMessagerBuilder(Exception exception)
        {
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine($"ErrMsg: {exception.Message}");

            AddExceptionInfo(messageBuilder, exception);

            return messageBuilder;
        }

        private static void AddExceptionInfo(StringBuilder builder, Exception exception,
            int limitedInnerException = LIMITED_INNER_EXCEPTION)
        {
            builder.AppendLine();
            builder.AppendLine("EXCEPTION INFO");
            builder.AppendLine($"Message: {exception.Message}");
            builder.AppendLine($"Source: {exception.Source}");
            builder.AppendLine($"TargetSite: {exception.TargetSite}");
            builder.AppendLine($"StackTrace: {exception.StackTrace}");
            if (exception.InnerException != null && limitedInnerException > 0)
            {
                limitedInnerException--;
                AddExceptionInfo(builder, exception.InnerException, limitedInnerException);
            }
        }
    }
}
