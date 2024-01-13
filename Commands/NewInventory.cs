using Discord.WebSocket;
using TheRemembererDiscordBot.CommandComponents;
using TheRemembererDiscordBot.UserData;

namespace TheRemembererDiscordBot.Commands
{
    public class NewInventory : Command
    {
        public override string CommandDescription() => "Name and create a new inventory.";
        public override List<CommandArgument> CommandArguments(SocketMessage message) => new() { new("Name of the new inventory", new()) };
        public override async Task CommandAction(SocketMessage message, List<object> args)
        {
            string nameOfNewInventory = args[0].ToString() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(nameOfNewInventory))
            {
                await Respond(message, "The name of the inventory cannot be whitespace or empty.");
                return;
            }

            SaveData thisUserData = SaveData.GetSaveData(message.Author.Id);

            if (thisUserData.Inventories.ContainsKey(nameOfNewInventory))
            {
                await Respond(message, "You already have an inventory with that name.");
                return;
            }

            thisUserData.Inventories.Add(nameOfNewInventory, new());

            await Respond(message, "The inventory `" + nameOfNewInventory + "` has been created successfully.");
        }
    }
}
