using Discord.WebSocket;

namespace TheRemembererDiscordBot.Commands
{
    public class Ping : Command
    {
        public override string CommandDescription() => "Test to see if the discord bot can properly receive and respond to commands.";
        public override async Task CommandAction(SocketMessage message, List<object> args) => await Respond(message, "Pong!");
    }
}
