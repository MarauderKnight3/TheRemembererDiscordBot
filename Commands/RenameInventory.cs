using Discord.WebSocket;
using TheRemembererDiscordBot.CommandComponents;
using TheRemembererDiscordBot.UserData;

namespace TheRemembererDiscordBot.Commands
{
    public class RenameInventory : Command
    {
        public override string CommandDescription() => "Rename an inventory.";
        public override List<CommandArgument> CommandArguments(SocketMessage message) => new() { new("Name of inventory to rename", SaveData.GetSaveData(message.Author.Id).Inventories.Keys.ToList<object>()), new("New name of the inventory", new()) };
        public override async Task CommandAction(SocketMessage message, List<object> args)
        {
            string oldName = args[0].ToString() ?? string.Empty;
            string newName = args[1].ToString() ?? string.Empty;

            int checkStatus = NewInventory.CheckNewInventoryName(message, newName);

            if (checkStatus != 0)
            {
                await Respond(message, NewInventory.CheckNewInventoryNameErrorMessage(checkStatus));
                return;
            }

            SaveData thisUserData = SaveData.GetSaveData(message.Author.Id);

            Inventory targetInventory = thisUserData.Inventories[oldName];

            thisUserData.Inventories.Add(newName, targetInventory);

            thisUserData.Inventories.Remove(oldName);

            await Respond(message, "The inventory `" + oldName + "` has been renamed successfully to `" + newName + "`.");
        }

    }
}
