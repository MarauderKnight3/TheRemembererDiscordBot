using Discord.WebSocket;
using TheRemembererDiscordBot.CommandComponents;
using TheRemembererDiscordBot.UserData;

namespace TheRemembererDiscordBot.Commands
{
    public class NewItemType : Command
    {
        public override string CommandDescription() => "Define a new type of item.";
        public override List<CommandArgument> CommandArguments(SocketMessage message) => new() { new("Name of this sort of item", new(), true), new("Weight of this sort of item", new() { 0, 1000000 }) };
        public override async Task CommandAction(SocketMessage message, List<object> args)
        {
            string name = args[0].ToString() ?? string.Empty;
            int weight = (int)args[1];

            int checkStatus = CheckNewItemTypeName(message, name);

            if (checkStatus != 0)
            {
                await Respond(message, CheckNewItemTypeNameErrorMessage(checkStatus));
                return;
            }

            SaveData thisUserData = SaveData.GetSaveData(message.Author.Id);

            if (thisUserData.ItemTypes.Count >= 6000)
            {
                await Respond(message, "You cannot have more than 6000 item types.");
                return;
            }

            thisUserData.ItemTypes.Add(new(name, weight));

            await Respond(message, "The item type `" + name + "` has been created successfully.");
        }

        public static int CheckNewItemTypeName(SocketMessage message, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return 1;
            if (name.Length >= 25)
                return 2;
            if (SaveData.GetSaveData(message.Author.Id).ItemTypes.Exists(x => x.Name == name))
                return 3;
            return 0;
        }

        public static string CheckNewItemTypeNameErrorMessage(int code) => code switch
        {
            1 => "The name of the item type cannot be whitespace or empty.",
            2 => "The name of the item type must be less than 25 characters.",
            3 => "You already have an item type with that name.",
            _ => ""
        };
    }
}
