using Discord.WebSocket;
using TheRemembererDiscordBot.CommandComponents;
using TheRemembererDiscordBot.UserData;

namespace TheRemembererDiscordBot.Commands
{
    public class DeleteInventory : Command
    {
        public override string CommandDescription() => "Delete an inventory.";
        public override List<CommandArgument> CommandArguments(SocketMessage message) => new() { new("Name of inventory to delete", SaveData.GetSaveData(message.Author.Id).Inventories.Keys.ToList<object>()), new("Confirm", new() { "confirm" }, mayBeSkipped: true) };
        public override async Task CommandAction(SocketMessage message, List<object> args)
        {
            if (args.Count < 1 || args[0] == null || args[0] is not string inventoryName)
                return;

            if (args.Count < 2)
            {
                await Respond(message, "Deleting an inventory will erase all of the data therein permanently. Are you sure? If so, then run the command with \"confirm\" after the name of the inventory you'd like to delete.");
                return;
            }

            try
            {
                SaveData.GetSaveData(message.Author.Id).Inventories.Remove(inventoryName);
                await Respond(message, "The inventory named `" + inventoryName + "` has been deleted successfully.");
            }
            catch
            {
                await Respond(message, "There was an error.");
            }
        }
    }
}
