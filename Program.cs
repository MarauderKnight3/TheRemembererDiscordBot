using Discord;
using Discord.Net;
using Discord.WebSocket;
using System.Text.Json;

namespace TheRemembererDiscordBot
{
    public class Program
    {
        private static Task Main() => new Program().MainAsync();
        private readonly DiscordShardedClient _client = new(DSC);
        private static readonly DiscordSocketConfig DSC = new() { LogGatewayIntentWarnings = false, MessageCacheSize = 0 };
        public static readonly List<Command> Commands = new();

        private async Task MainAsync()
        {
            _client.Log += Log;
            _client.ShardReady += ClientReady;
            _client.SlashCommandExecuted += SlashCommandHandler;

            string token = File.ReadAllText("token.txt");
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private static async Task ClientReady(DiscordSocketClient client)
        {
            foreach (Command command in Commands)
            {
                SlashCommandBuilder commandBuilder = new();
                commandBuilder.WithName(command.CommandName());
                commandBuilder.WithDescription(command.CommandDescription());
                try
                {
                    await client.CreateGlobalApplicationCommandAsync(commandBuilder.Build());
                }
                catch (ApplicationCommandException exception)
                {
                    var json = JsonSerializer.Serialize(exception.Errors);
                    Console.WriteLine(json);
                }
            }
        }

        private static async Task SlashCommandHandler(SocketSlashCommand command)
        {
            await command.RespondAsync($"You executed {command.Data.Name}");
        }
    }
}
