using Discord.WebSocket;
using TheRemembererDiscordBot.CommandComponents;
using TheRemembererDiscordBot.UserData;

namespace TheRemembererDiscordBot.Commands
{
    public class NewInventory : Command
    {
        public override string CommandDescription() => "Name and create a new inventory.";
        public override List<CommandArgument> CommandArguments() => new() { new("Name of the new inventory", new()) };
        public override async Task CommandAction(SocketMessage inputMessage, List<object> args)
        {
            string nameOfNewInventory = args[0].ToString() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(nameOfNewInventory))
            {
                await Respond(inputMessage, "The name of the inventory cannot be whitespace or empty.");
                return;
            }

            SaveData thisUserData = SaveData.GetSaveData(inputMessage.Author.Id);

            if (thisUserData.Inventories.ContainsKey(nameOfNewInventory))
            {
                await Respond(inputMessage, "You already have an inventory with that name.");
                return;
            }

            thisUserData.Inventories.Add(nameOfNewInventory, new());

            await Respond(inputMessage, "The inventory `" + nameOfNewInventory + "` has been created successfully.");
        }
    }
}
