using Discord.WebSocket;
using TheRemembererDiscordBot.CommandComponents;
using TheRemembererDiscordBot.UserData;

namespace TheRemembererDiscordBot.Commands
{
    public class RenameItem : Command
    {
        public override string CommandDescription() => "Rename an item type.";
        public override List<CommandArgument> CommandArguments(SocketMessage message) => new() { new("Name of item type to rename", SaveData.GetSaveData(message.Author.Id).ItemTypes.Select(x => x.Name).ToList<object>(), forceNotCustom: true), new("New name of the item type", new()) };
        public override async Task CommandAction(SocketMessage message, List<object> args)
        {
            string oldName = args[0].ToString() ?? string.Empty;
            string newName = args[1].ToString() ?? string.Empty;

            int checkStatus = NewItem.CheckNewItemTypeName(message, newName);

            if (checkStatus != 0)
            {
                await Respond(message, NewItem.CheckNewItemTypeNameErrorMessage(checkStatus));
                return;
            }

            SaveData thisUserData = SaveData.GetSaveData(message.Author.Id);

            ItemType targetItemType = thisUserData.ItemTypes.First(x => x.Name == oldName);

            targetItemType.SetName(newName);

            await Respond(message, "The item type `" + oldName + "` has been renamed successfully to `" + newName + "`.");
        }
    }
}
