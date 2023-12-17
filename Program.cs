using Discord;
using Discord.WebSocket;

namespace RuneDungeon
{
    public class Program
    {
        private static Task Main() => new Program().MainAsync();
        private readonly DiscordShardedClient _client = new(DSC);
        private static readonly DiscordSocketConfig DSC = new() { LogGatewayIntentWarnings = false, MessageCacheSize = 0 };

        private async Task MainAsync()
        {
            _client.Log += Log;

            string token = File.ReadAllText("token.txt");
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
