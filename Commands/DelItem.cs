using Discord.WebSocket;
using TheRemembererDiscordBot.CommandComponents;
using TheRemembererDiscordBot.UserData;

namespace TheRemembererDiscordBot.Commands
{
    public class DelItem : Command
    {
        public override string CommandDescription() => "Delete an item type.";
        public override List<CommandArgument> CommandArguments(SocketMessage message) => new() { new("Name of item type to delete", SaveData.GetSaveData(message.Author.Id).ItemTypes.Select(x => x.Name).ToList<object>()), new("Confirm", new() { "confirm" }, mayBeSkipped: true) };
        public override async Task CommandAction(SocketMessage message, List<object> args)
        {
            if (args.Count < 1 || args[0] == null || args[0] is not string itemTypeName)
                return;

            if (args.Count < 2)
            {
                await Respond(message, "Deleting an item type is permanent, and all instances of that type of item will disappear from all inventories permanently. Are you sure? If so, then run the command with \"confirm\" after the name of the item type you'd like to delete.");
                return;
            }

            try
            {
                SaveData thisUserData = SaveData.GetSaveData(message.Author.Id);

                ItemType itemType = thisUserData.ItemTypes.First(x => x.Name == itemTypeName);

                foreach (Inventory inventory in thisUserData.Inventories.Values)
                {
                    Item? item = inventory.Items.FirstOrDefault(x => x.Type == itemType);

                    if (item == null) continue;

                    inventory.Items.Remove(item);
                }

                thisUserData.ItemTypes.Remove(itemType);

                await Respond(message, "The item type named `" + itemTypeName + "` has been deleted successfully.");
            }
            catch
            {
                await Respond(message, "There was an error.");
            }
        }
    }
}
