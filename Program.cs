using Discord;
using Discord.WebSocket;

namespace RuneDungeon
{
    public class Program
    {
        private static Task Main() => new Program().MainAsync(); //Running the program runs MainAsync()
        private readonly DiscordShardedClient _client = new(DSC); //Make a discord sharded client with these settings. Sharded client means it runs multiple bot instances at once, each for a subset of servers.
        private static readonly DiscordSocketConfig DSC = new()
        {
            LogGatewayIntentWarnings = false, //Gets rid of warnings I don't care about.
            MessageCacheSize = 0 //An attempt to stop the bot from caching any messages.
        };
        private async Task MainAsync() //This method starts the bot client.
        {
            _client.Log += Log; //This hooks the Log() method (defined just below this MainAsync method definition) into the discord api log event.
            //_client.MessageReceived += MessageReceived; //Same for receiving messages. I use MessageReceived() to do all the game interactions.

            string token = File.ReadAllText("token.txt"); //Get's the bots password.
            await _client.LoginAsync(TokenType.Bot, token); //Logs in.
            await _client.StartAsync(); //Makes the bot start thinking/connecting?

            await Task.Delay(-1); //Waits indefinitely for actions.
        }

        private Task Log(LogMessage msg) //This method is responsible for putting all the Discord api logs into my console.
        {
            Console.WriteLine(msg.ToString()); //This writes to console.
            return Task.CompletedTask; //Many methods return a Task to let the rest of the code know when it finishes.
        }
    }
}