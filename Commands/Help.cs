using Discord.WebSocket;
using TheRemembererDiscordBot.CommandComponents;

namespace TheRemembererDiscordBot.Commands
{
    public class Help : Command
    {
        public override string CommandDescription() => "List commands, or arguments for a selected command.";
        public override List<CommandArgument> CommandArguments() => new() { new("Command to help with", Program.Commands.Select(x => x.CommandName()).ToList<object>(), false, true) };
        public override async Task<object?> CommandAction(SocketMessage inputMessage, List<object> args)
        {
            if (args.Count == 0)
            {
                string output = "Prompt about a command with help <command> to see some details on how it can be used.\n";
                string tooManyCommands = "More commands not shown due to message limit.";
                foreach (Command command in Program.Commands)
                {
                    string thisCommandText = command + "\n";

                    if (output.Length + thisCommandText.Length + tooManyCommands.Length > 2000)
                    {
                        output += tooManyCommands;
                        break;
                    }

                    output += thisCommandText;
                }
                try
                {
                    await inputMessage.Channel.SendMessageAsync(output);
                }
                catch { }
            }
            else
            {
                Command? command = Program.Commands.FirstOrDefault(x => (string)args[0] == x.CommandName());

                try
                {
                    if (command != null)
                        await inputMessage.Channel.SendMessageAsync(HelpWithCommand(command));
                }
                catch { }
            }

            return null;
        }

        public static string HelpWithCommand(Command command)
        {
            string output = command.ToString() + "\nHere are the arguments:\n";

            if (command.CommandArguments().Count == 0)
            {
                output += command.CustomCommandArgumentsDescription();
            }

            foreach (CommandArgument arg in command.CommandArguments())
            {
                output += "***" + arg + "***: *Expects " + arg.ArgumentType switch
                {
                    CommandArgument.ArgType.PositiveIntegerRange =>
                        "a whole number between " + (string)arg.ExpectedInputs[0] + " and " + (string)arg.ExpectedInputs[1],

                    CommandArgument.ArgType.PositiveIntegerRangeOrText or CommandArgument.ArgType.PositiveIntegerRangeOrTextConcat =>
                        "a whole number between " + (string)arg.ExpectedInputs[0] + " and " + (string)arg.ExpectedInputs[1]
                        + " or any of the following: " + string.Join(", ", arg.ExpectedInputs.GetRange(2, arg.ExpectedInputs.Count - 2)),

                    CommandArgument.ArgType.Text or CommandArgument.ArgType.TextConcat =>
                        "any of the following: " + string.Join(", ", arg.ExpectedInputs),

                    _ => "text (doesn't need to match an expected value)"
                } + "* " + (arg.MayBeSkipped ? "**(This argument can be skipped)**" : "") + "\n";
            }

            return output;
        }

    }
}
