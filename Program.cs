using Discord;
using Discord.WebSocket;
using TheRemembererDiscordBot.UserData;

namespace TheRemembererDiscordBot
{
    public class Program
    {
        public static readonly List<Command> Commands = new()
        {
            new Commands.Help(),
            new Commands.Ping(),
            new Commands.CoinFlip(),
            new Commands.DiceRoll(),

            new Commands.NewInventory(),
            new Commands.ListInventories(),
            new Commands.RenameInventory(),
            new Commands.DeleteInventory(),
            new Commands.DeleteAllInventories(),

            new Commands.NewItemType(),
            new Commands.ListItemTypes(),
        };

        private static Task Main() => new Program().MainAsync();
        private readonly DiscordShardedClient _client = new(DSC);
        private static readonly DiscordSocketConfig DSC = new()
        {
            LogGatewayIntentWarnings = false,
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent,
            MessageCacheSize = 0
        };

        private async Task MainAsync()
        {
            _client.Log += Log;
            //_client.ShardReady += ClientReady;
            _client.MessageReceived += MessageReceived;
            //_client.SlashCommandExecuted += SlashCommandHandler;

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

        //private static async Task ClientReady(DiscordSocketClient client)
        //{
        //    foreach (Command command in Commands)
        //    {
        //        SlashCommandBuilder commandBuilder = new();
        //        commandBuilder.WithName(command.CommandName());
        //        commandBuilder.WithDescription(command.CommandDescription());
        //        try
        //        {
        //            await client.CreateGlobalApplicationCommandAsync(commandBuilder.Build());
        //        }
        //        catch (HttpException exception)
        //        {
        //            var json = JsonSerializer.Serialize(exception.Errors);
        //            Console.WriteLine(json);
        //        }
        //    }
        //}

        private static async Task<Task> MessageReceived(SocketMessage message)
        {
            if (message.Author.IsBot || message.Author.IsWebhook || message == null || message.Channel == null)
                return Task.CompletedTask;

            if (message.Content.StartsWith(";"))
            {
                List<string> input = CommandUtils.SeparateArguments(message.Content[1..]);

                Command? command = Commands.FirstOrDefault(x => input[0] == x.CommandName());
                if (command != null)
                {
                    input.RemoveAt(0);

                    List<object> polished = command.ParseArguments(message, input);

                    if (polished.Count > 0 && polished[0] is bool)
                    {
                        try
                        {
                            await message.Channel.SendMessageAsync((string)polished[1] + "\n" + TheRemembererDiscordBot.Commands.Help.HelpWithCommand(message, command));
                        }
                        catch { }
                        return Task.CompletedTask;
                    }
                    await command.CommandAction(message, polished);
                }
            }

            SaveData.Write(message.Author.Id, SaveData.GetSaveData(message.Author.Id));
            return Task.CompletedTask;
        }

        //private static async Task SlashCommandHandler(SocketSlashCommand command)
        //{
        //    await command.RespondAsync($"You executed {command.Data.Name}");
        //}
    }
}
