using System.Text;
using System.Text.RegularExpressions;

namespace TheRemembererDiscordBot
{
    public static class CommandUtils
    {
        private static readonly Regex ArgumentSeparator = new("[\"].*?[\"]|\\S+");

        public static List<string> SeparateArguments(string message) => ArgumentSeparator.Matches(message).Select(x => x.Value.StartsWith("\"") && x.Value.EndsWith("\"") ? x.Value[1..^1] : x.Value).ToList();

        public static bool IsPositiveArabicNumber(this string value)
        {
            if (value == null || value == "")
                return false;

            for (int i = 0; i < value.Length; i++)
                if ((value[i] ^ '0') > 9)
                    return false;

            return true;
        }

        public static string TruncateJoin(string separator, string terminator, List<string> values, int maxLength) => TruncateJoin(separator, terminator, values as IEnumerable<string>, maxLength);

        public static string TruncateJoin(string separator, string terminator, IEnumerable<string> values, int maxLength)
        {
            StringBuilder result = new();
            int currentLength = 0;

            foreach (string value in values)
            {
                if (currentLength + value.Length + separator.Length > maxLength)
                {
                    result.Append(value + terminator);
                    break;
                }

                result.Append(value + separator);
                currentLength += value.Length + separator.Length;
            }

            return result.ToString().TrimEnd(separator.ToCharArray());
        }
    }
}
