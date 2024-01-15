using Discord.WebSocket;
using TheRemembererDiscordBot.CommandComponents;
using TheRemembererDiscordBot.UserData;

namespace TheRemembererDiscordBot.Commands
{
    public class Consume : Command
    {
        public override string CommandDescription() => "Take an item, or multiple, of a specified type, from a specified inventory.";
        public override List<CommandArgument> CommandArguments(SocketMessage message) => new()
        {
            new("Inventory to take from", SaveData.GetSaveData(message.Author.Id).Inventories.Keys.ToList<object>(), forceNotCustom: true),
            new("Item to take", SaveData.GetSaveData(message.Author.Id).ItemTypes.Select(x => x.Name).ToList<object>(), true, forceNotCustom: true),
            new("Amount to take", new() { 1, 1000000 }, mayBeSkipped: true)
        };
        public override async Task CommandAction(SocketMessage message, List<object> args)
        {
            string inventoryName = args[0].ToString() ?? string.Empty;
            string itemName = args[1].ToString() ?? string.Empty;
            int amount;

            if (args.Count > 2)
                amount = (int)args[2] * -1;
            else
                amount = -1;

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

            await Respond(message, inventoryName + " lost " + (amount * -1) + " " + itemName + " and now has " + item.Count + ".");
        }
    }
}
