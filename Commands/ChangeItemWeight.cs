﻿using Discord.WebSocket;
using TheRemembererDiscordBot.CommandComponents;
using TheRemembererDiscordBot.UserData;

namespace TheRemembererDiscordBot.Commands
{
    public class ChangeItemWeight : Command
    {
        public override string CommandDescription() => "Change the weight of an item type.";
        public override List<CommandArgument> CommandArguments(SocketMessage message) => new() { new("Name of item type to change", SaveData.GetSaveData(message.Author.Id).ItemTypes.Select(x => x.Name).ToList<object>(), forceNotCustom: true), new("New weight of the item type", new() { 0, 1000000 }) };
        public override async Task CommandAction(SocketMessage message, List<object> args)
        {
            string itemTypeName = args[0].ToString() ?? string.Empty;
            int newWeight = (int)args[1];

            SaveData thisUserData = SaveData.GetSaveData(message.Author.Id);

            ItemType targetItemType = thisUserData.ItemTypes.First(x => x.Name == itemTypeName);

            targetItemType.SetWeight(newWeight);

            foreach (KeyValuePair<string, Inventory> inventory in thisUserData.Inventories)
            {
                foreach (Item item in inventory.Value.Items.Where(x => x.Type.Name == itemTypeName))
                {
                    item.ChangeType(targetItemType);
                }
            }

            await Respond(message, "The weight of the item type `" + itemTypeName + "` has been changed successfully.");
        }
    }
}
