using Discord.WebSocket;
using TheRemembererDiscordBot.CommandComponents;

namespace TheRemembererDiscordBot.Commands
{
    public class Help : Command
    {
        public override string CommandDescription() => "List commands, or arguments for a selected command.";
        public override List<CommandArgument> CommandArguments(SocketMessage message) => new() { new("Command to help with", Program.Commands.Select(x => x.CommandName()).ToList<object>(), mayBeSkipped: true) };
        public override async Task CommandAction(SocketMessage message, List<object> args)
        {
            if (args.Count == 0)
            {
                string response = "Prompt about a command with `help <command>` to see some details on how it can be used.\n";
                string tooManyCommands = "More commands not shown due to message limit.";
                foreach (Command command in Program.Commands)
                {
                    string thisCommandText = command + "\n";

                    if (response.Length + thisCommandText.Length + tooManyCommands.Length > 2000)
                    {
                        response += tooManyCommands;
                        break;
                    }

                    response += thisCommandText;
                }
                await Respond(message, response);
            }
            else
            {
                Command? command = Program.Commands.FirstOrDefault(x => (string)args[0] == x.CommandName());

                if (command != null)
                    await Respond(message, HelpWithCommand(message, command));
            }
        }

        public static string HelpWithCommand(SocketMessage message, Command command)
        {
            string output = command.ToString() + "\nArguments, in order, are as follows:\n";

            if (command.CommandArguments(message).Count == 0)
            {
                output += command.CustomCommandArgumentsDescription();
            }

            foreach (CommandArgument arg in command.CommandArguments(message))
            {
                output += "`" + arg + "`: *Expects " + arg.ArgumentType switch
                {
                    CommandArgument.ArgType.PositiveIntegerRange =>
                        "a whole number between " + arg.ExpectedInputs[0].ToString() + " and " + arg.ExpectedInputs[1].ToString(),

                    CommandArgument.ArgType.PositiveIntegerRangeOrText or CommandArgument.ArgType.PositiveIntegerRangeOrTextConcat =>
                        "a whole number between " + arg.ExpectedInputs[0].ToString() + " and " + arg.ExpectedInputs[1].ToString()
                        + " or any of the following: " + CommandUtils.TruncateJoin(", ", "...", arg.ExpectedInputs.GetRange(2, arg.ExpectedInputs.Count - 2).OfType<string>(), 200),

                    CommandArgument.ArgType.Text or CommandArgument.ArgType.TextConcat =>
                        "any of the following: " + CommandUtils.TruncateJoin(", ", "...", arg.ExpectedInputs.OfType<string>(), 200),

                    _ => "text (doesn't need to match an expected value)"
                } + "* " + (arg.MayBeSkipped ? "**(This argument can be skipped)**" : "") + "\n";
            }

            return output;
        }
    }
}
