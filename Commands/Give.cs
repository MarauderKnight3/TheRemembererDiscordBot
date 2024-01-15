using Discord.WebSocket;
using TheRemembererDiscordBot.CommandComponents;
using TheRemembererDiscordBot.UserData;

namespace TheRemembererDiscordBot.Commands
{
    public class Give : Command
    {
        public override string CommandDescription() => "Put an item, or multiple, of a specified type, into a specified inventory.";
        public override List<CommandArgument> CommandArguments(SocketMessage message) => new()
        {
            new("Inventory to give to", SaveData.GetSaveData(message.Author.Id).Inventories.Keys.ToList<object>(), forceNotCustom: true),
            new("Item to give", SaveData.GetSaveData(message.Author.Id).ItemTypes.Select(x => x.Name).ToList<object>(), true, forceNotCustom: true),
            new("Amount to give", new() { 1, 1000000 }, mayBeSkipped: true)
        };
        public override async Task CommandAction(SocketMessage message, List<object> args)
        {
            string inventoryName = args[0].ToString() ?? string.Empty;
            string itemName = args[1].ToString() ?? string.Empty;
            int amount;

            if (args.Count > 2)
                amount = (int)args[2];
            else
                amount = 1;

            SaveData thisUserData = SaveData.GetSaveData(message.Author.Id);

            Inventory targetInventory = thisUserData.Inventories[inventoryName];
            ItemType itemType = thisUserData.ItemTypes.First(x => x.Name == itemName);
            Item? item = targetInventory.Items.FirstOrDefault(x => x.Type.Name == itemName);

            if (item == null)
            {
                item = new(itemType, amount);
                targetInventory.Items.Add(item);
            }
            else
            {
                item.ModifyCount(amount);
            }

            if (item.Count == 0)
            {
                targetInventory.Items.Remove(item);
            }

            await Respond(message, inventoryName + " gained " + amount + " " + itemName + " and now has " + item.Count + ".");
        }
    }
}
