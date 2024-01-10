using Discord.WebSocket;

namespace TheRemembererDiscordBot.Commands
{
    public class Ping : Command
    {
        public override string CommandDescription() => "Test to see if the discord bot can properly receive and respond to commands.";
        public override async Task<object?> CommandAction(SocketMessage inputMessage, List<object> args)
        {
            await Respond(inputMessage, "Pong!");

            return null;
        }
    }
}
