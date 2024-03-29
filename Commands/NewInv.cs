﻿using Discord.WebSocket;
using TheRemembererDiscordBot.CommandComponents;
using TheRemembererDiscordBot.UserData;

namespace TheRemembererDiscordBot.Commands
{
    public class NewInv : Command
    {
        public override string CommandDescription() => "Name and create a new inventory.";
        public override List<CommandArgument> CommandArguments(SocketMessage message) => new() { new("Name of the new inventory", new()) };
        public override async Task CommandAction(SocketMessage message, List<object> args)
        {
            string name = args[0].ToString() ?? string.Empty;

            int checkStatus = CheckNewInventoryName(message, name);

            if (checkStatus != 0)
            {
                await Respond(message, CheckNewInventoryNameErrorMessage(checkStatus));
                return;
            }

            SaveData thisUserData = SaveData.GetSaveData(message.Author.Id);

            if (thisUserData.Inventories.Count >= 25)
            {
                await Respond(message, "You cannot have more than 25 inventories.");
                return;
            }

            thisUserData.Inventories.Add(name, new());

            await Respond(message, "The inventory `" + name + "` has been created successfully.");
        }

        public static int CheckNewInventoryName(SocketMessage message, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return 1;
            if (name.Length >= 25)
                return 2;
            if (SaveData.GetSaveData(message.Author.Id).Inventories.ContainsKey(name))
                return 3;
            return 0;
        }

        public static string CheckNewInventoryNameErrorMessage(int code) => code switch
        {
            1 => "The name of the inventory cannot be whitespace or empty.",
            2 => "The name of the inventory must be less than 25 characters.",
            3 => "You already have an inventory with that name.",
            _ => ""
        };
    }
}
