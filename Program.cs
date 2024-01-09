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
        private static readonly DiscordSocketConfig DSC = new()
        {
            LogGatewayIntentWarnings = false,
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent,
            MessageCacheSize = 0
        };
        public static readonly List<Command> Commands = new()
        {
            new Commands.Ping()
        };

        private async Task MainAsync()
        {
            _client.Log += Log;
            _client.ShardReady += ClientReady;
            _client.MessageReceived += MessageReceived;
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
                catch (HttpException exception)
                {
                    var json = JsonSerializer.Serialize(exception.Errors);
                    Console.WriteLine(json);
                }
            }
        }

        private static async Task<Task> MessageReceived(SocketMessage msg)
        {
            if (msg.Author.IsBot || msg.Author.IsWebhook || msg == null || msg.Channel == null)
                return Task.CompletedTask;

            if (msg.Content.StartsWith(";"))
            {
                string promptedCommand = msg.Content[1..].Split(" ", StringSplitOptions.RemoveEmptyEntries)[0];

                foreach (Command command in Commands)
                    if (command.CommandName() == promptedCommand)
                        await command.CommandAction(msg, new List<object>());
            }

            return Task.CompletedTask;
        }

        private static async Task SlashCommandHandler(SocketSlashCommand command)
        {
            await command.RespondAsync($"You executed {command.Data.Name}");
        }
    }
}
