using System.Text.RegularExpressions;

namespace TheRemembererDiscordBot
{
    public static class CommandUtils
    {
        private static readonly Regex ArgumentSeparator = new("[\"].*?[\"]|\\S+");

        public static List<string> SeparateArguments(string message) => ArgumentSeparator.Matches(message).Select(x => x.Value).ToList();

        public static bool IsPositiveArabicNumber(this string value)
        {
            if (value == null || value == "")
                return false;

            for (int i = 0; i < value.Length; i++)
                if ((value[i] ^ '0') > 9)
                    return false;

            return true;
        }
    }
}
