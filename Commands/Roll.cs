using Discord.WebSocket;
using TheRemembererDiscordBot.CommandComponents;

namespace TheRemembererDiscordBot.Commands
{
    public class Roll : Command
    {
        public override string CommandDescription() => "Rolls a D10, or a die with as many faces as you specify.";
        public override List<CommandArgument> CommandArguments(SocketMessage message) => new() { new("Faces on the die", new() { 2, 1000 }, mayBeSkipped: true) };
        public override async Task CommandAction(SocketMessage message, List<object> args)
        {
            string response = DiceTexts[Random.Shared.Next(DiceTexts.Count)];
            int faces = args.Count == 1 ? (int)args[0] : 10;
            int result = Random.Shared.Next(faces) + 1;
            response += result == 11 || result.ToString().StartsWith("8") ? "an " : "a ";
            response += result;
            response += result >= faces * 0.8 ? "!" : ".";

            await Respond(message, response);
        }

        public static readonly List<string> DiceTexts = new()
        {
            "You rolled ",
            "That will be ",
            "That's ",
            "Looks like ",
            "It's "
        };
    }
}
