using Discord.WebSocket;
using System.Text.RegularExpressions;
using static TheRemembererDiscordBot.CommandArgument;

namespace TheRemembererDiscordBot
{
    public class Command
    {
        public virtual string? CommandName() => null;
        public virtual string? CommandDescription() => null;
        public virtual List<CommandArgument> CommandArguments() => new();
        public virtual async Task<object?> CommandAction(SocketMessage inputMessage, List<object> args)
        {
            try
            {
                await inputMessage.Channel.SendMessageAsync("This is undefined command behavior.");
            }
            catch { }

            return null;
        }

        public List<object> ParseArguments(List<string> arguments) => ParseArguments(arguments, CommandArguments());

        public static List<object> ParseArguments(List<string> arguments, List<CommandArgument> argumentDetails)
        {
            if (argumentDetails.Count == 0)
                return arguments.Cast<object>().ToList();
            List<object> polished = new();
            for (int i = 0; i < argumentDetails.Count; i++)
            {
                if (arguments.Count == 0 && !argumentDetails[i].MayBeSkipped)
                    return new List<object> { false };
                if (arguments.Count == 0)
                    break;
                if (argumentDetails[i].ArgumentType is ArgType.CustomText)
                {
                    polished.Add(arguments[0]);
                    arguments.RemoveAt(0);
                    continue;
                }
                if (argumentDetails[i].ArgumentType is ArgType.PositiveIntegerRangeOrText or ArgType.PositiveIntegerRangeOrTextConcat or ArgType.PositiveIntegerRange && arguments[0].IsPositiveArabicNumber())
                {
                    if (!int.TryParse(arguments[0], out int n) || (int)argumentDetails[i].ExpectedInputs.ElementAt(0) > n || n > (int)argumentDetails[i].ExpectedInputs.ElementAt(1))
                        return new List<object> { false };
                    polished.Add(n);
                    arguments.RemoveAt(0);
                    continue;
                }
                else if (argumentDetails[i].ArgumentType is ArgType.PositiveIntegerRange)
                {
                    return new List<object> { false };
                }
                string query = arguments[0];
                arguments.RemoveAt(0);
                if (argumentDetails[i].ArgumentType is ArgType.TextConcat or ArgType.PositiveIntegerRangeOrTextConcat or ArgType.CustomTextConcat)
                {
                    while ((arguments.Count > 0 && new Regex("\\D").IsMatch(arguments[0])) || i == argumentDetails.Count - 1)
                    {
                        query += " " + arguments[0];
                        arguments.RemoveAt(0);
                    }
                    if (argumentDetails[i].ArgumentType is ArgType.CustomTextConcat)
                    {
                        polished.Add(query);
                        continue;
                    }
                }

                IEnumerable<string> matches = argumentDetails[i].ExpectedInputs.Select(x => (string)x).Where(x => x.StartsWith(query, StringComparison.OrdinalIgnoreCase));

                if (!matches.Any())
                    return new List<object> { false };
                if (matches.Count() == 1 && matches.FirstOrDefault() is string onlyMatch)
                    polished.Add(onlyMatch);
                else
                {
                    string? match = matches.Where(x => x.Equals(query, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                    if (match is null)
                        return new List<object> { false };
                    polished.Add(match);
                }
            }
            return polished;
        }
    }
}
