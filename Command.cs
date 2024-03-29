﻿using Discord.WebSocket;
using System.Text.RegularExpressions;
using TheRemembererDiscordBot.CommandComponents;

namespace TheRemembererDiscordBot
{
    public class Command
    {
        private string? _CommandName;
        public string CommandName()
        {
            _CommandName ??= GetType().Name.ToLower();
            return _CommandName;
        }
        public virtual string? CommandDescription() => null;
        public virtual string CustomCommandArgumentsDescription() => "Command does not have any arguments.";
        public virtual List<CommandArgument> CommandArguments(SocketMessage message) => new();
        public virtual async Task CommandAction(SocketMessage message, List<object> args) => await Respond(message, "This is undefined command behavior.");

        public static async Task<Discord.Rest.RestUserMessage?> Respond(SocketMessage message, string response)
        {
            try { return await message.Channel.SendMessageAsync(response); } catch { return null; }
        }

        public List<object> ParseArguments(SocketMessage message, List<string> arguments) => ParseArguments(arguments, CommandArguments(message));

        public static List<object> ParseArguments(List<string> arguments, List<CommandArgument> argumentDetails)
        {
            if (argumentDetails.Count == 0)
                return arguments.Cast<object>().ToList();
            List<object> polished = new();
            for (int i = 0; i < argumentDetails.Count; i++)
            {
                if (arguments.Count == 0 && !argumentDetails[i].MayBeSkipped)
                    return new List<object> { false, "Missing required input for argument \"" + argumentDetails[i] + "\"" };
                if (arguments.Count == 0)
                    break;
                if (argumentDetails[i].ArgumentType is CommandArgument.ArgType.CustomText)
                {
                    polished.Add(arguments[0]);
                    arguments.RemoveAt(0);
                    continue;
                }
                if (argumentDetails[i].ArgumentType is CommandArgument.ArgType.PositiveIntegerRangeOrText or CommandArgument.ArgType.PositiveIntegerRangeOrTextConcat or CommandArgument.ArgType.PositiveIntegerRange && arguments[0].IsPositiveArabicNumber())
                {
                    if (!int.TryParse(arguments[0], out int n) || (int)argumentDetails[i].ExpectedInputs.ElementAt(0) > n || n > (int)argumentDetails[i].ExpectedInputs.ElementAt(1))
                        return new List<object> { false, "Input for argument \"" + argumentDetails[i] + "\" was not a number in the valid range." };
                    polished.Add(n);
                    arguments.RemoveAt(0);
                    continue;
                }
                else if (argumentDetails[i].ArgumentType is CommandArgument.ArgType.PositiveIntegerRange)
                    return new List<object> { false, "Input for argument \"" + argumentDetails[i] + "\" was not a number." };

                string query = arguments[0];
                arguments.RemoveAt(0);
                if (argumentDetails[i].ArgumentType is CommandArgument.ArgType.TextConcat or CommandArgument.ArgType.PositiveIntegerRangeOrTextConcat or CommandArgument.ArgType.CustomTextConcat)
                {
                    while ((arguments.Count > 0 && new Regex("\\D").IsMatch(arguments[0])) || i == argumentDetails.Count - 1 && arguments.Count > 0)
                    {
                        query += " " + arguments[0];
                        arguments.RemoveAt(0);
                    }
                    if (argumentDetails[i].ArgumentType is CommandArgument.ArgType.CustomTextConcat)
                    {
                        polished.Add(query);
                        continue;
                    }
                }

                IEnumerable<string> matches = argumentDetails[i].ExpectedInputs.Select(x => (string)x).Where(x => x.StartsWith(query, StringComparison.OrdinalIgnoreCase));

                if (!matches.Any())
                    return new List<object> { false, "Input for argument \"" + argumentDetails[i] + "\" did not match an option." };
                if (matches.Count() == 1 && matches.FirstOrDefault() is string onlyMatch)
                    polished.Add(onlyMatch);
                else
                {
                    string? match = matches.Where(x => x.Equals(query, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                    if (match is null)
                        return new List<object> { false, "Shorthand input for argument \"" + argumentDetails[i] + "\" matched more than one option. Please be more specific." };
                    polished.Add(match);
                }
            }
            return polished;
        }

        public override string ToString() => "`" + CommandName() + "`: " + CommandDescription();
    }
}
