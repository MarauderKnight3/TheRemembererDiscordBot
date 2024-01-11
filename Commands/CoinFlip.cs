using Discord.WebSocket;

namespace TheRemembererDiscordBot.Commands
{
    public class CoinFlip : Command
    {
        public override string CommandDescription() => "Responds with Heads or Tails.";
        public override async Task CommandAction(SocketMessage inputMessage, List<object> args) => await Respond(inputMessage, Random.Shared.Next(2) == 0 ? "Heads." : "Tails.");
    }
}
