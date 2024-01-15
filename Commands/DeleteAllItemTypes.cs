using Discord.WebSocket;
using TheRemembererDiscordBot.CommandComponents;
using TheRemembererDiscordBot.UserData;

namespace TheRemembererDiscordBot.Commands
{
    public class DeleteAllItemTypes : Command
    {
        public override string CommandDescription() => "Delete all of your item types.";
        public override List<CommandArgument> CommandArguments(SocketMessage message) => new() { new("Confirm", new() { "confirm" }, mayBeSkipped: true) };
        public override async Task CommandAction(SocketMessage message, List<object> args)
        {
            if (args.Count < 1)
            {
                await Respond(message, "Deleting all your item types is permanent, and all items will also disappear from all inventories permanently. Are you sure? If so, then run the command with \"confirm\".");
                return;
            }

            try
            {
                SaveData thisUserData = SaveData.GetSaveData(message.Author.Id);

                foreach (Inventory inventory in thisUserData.Inventories.Values)
                    inventory.Items.Clear();

                thisUserData.ItemTypes.Clear();

                await Respond(message, "You have successfully deleted all of your item types.");
            }
            catch
            {
                await Respond(message, "There was an error.");
            }
        }
    }
}
