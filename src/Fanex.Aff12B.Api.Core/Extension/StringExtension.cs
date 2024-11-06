
namespace Fanex.Aff12B.Api.Core.Extension
{
    public static class StringExtension
    {
        public static bool IsMatch(this string a, string b)
        {
            return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
        }
    }
}
