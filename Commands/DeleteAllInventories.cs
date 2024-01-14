using Discord.WebSocket;
using TheRemembererDiscordBot.CommandComponents;
using TheRemembererDiscordBot.UserData;

namespace TheRemembererDiscordBot.Commands
{
    public class DeleteAllInventories : Command
    {
        public override string CommandDescription() => "Delete all of your inventories.";
        public override List<CommandArgument> CommandArguments(SocketMessage message) => new() { new("Confirm", new() { "confirm" }, mayBeSkipped: true) };
        public override async Task CommandAction(SocketMessage message, List<object> args)
        {
            if (args.Count < 1)
            {
                await Respond(message, "This command will delete ALL of your inventories permanently. It will not delete the item types you have made, but the inventories and their contents will be irrecoverable. Are you sure? If so, then run the command with \"confirm\".");
                return;
            }

            try
            {
                SaveData.GetSaveData(message.Author.Id).Inventories.Clear();
                await Respond(message, "You have successfully deleted all of your inventories.");
            }
            catch
            {
                await Respond(message, "There was an error.");
            }
        }
    }
}
