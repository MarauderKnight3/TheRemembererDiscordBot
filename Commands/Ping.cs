using Discord.WebSocket;

namespace TheRemembererDiscordBot
{
    public class Ping : Command
    {
        public override string CommandName() => "ping";
        public override string CommandDescription() => "Test to see if the discord bot can properly receive and respond to commands.";
        public override async Task<object?> CommandAction(SocketMessage inputMessage, List<object> args)
        {
            try
            {
                await inputMessage.Channel.SendMessageAsync("Pong!");
            }
            catch { }

            return null;
        }
    }
}
