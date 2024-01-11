using Discord.WebSocket;
using System.Text.Json;
using TheRemembererDiscordBot.CommandComponents;
using TheRemembererDiscordBot.UserData;

namespace TheRemembererDiscordBot.Commands
{
    public class NewDataType : Command
    {
        public override string CommandDescription() => "Structure a new data type for use in a table. If you don't supply JSON, example JSON will be given, which you can use to make your own.";
        public override List<CommandArgument> CommandArguments() => new() { new("JSON", new(), true, true) };
        public override async Task CommandAction(SocketMessage inputMessage, List<object> args)
        {
            if (args.Count == 0)
                await Respond(inputMessage, "Here is some example JSON you can use to make your own data type. When doing so, it may be compact or indented. Pass it as plain text into the command; all arguments will be taken as part of the JSON.\n" + UserDataType.Examples);
            else
            {
                string response;
                UserDataType? userDataType;

                try
                {
                    userDataType = JsonSerializer.Deserialize<UserDataType>((string)args[0]);
                    string invalid = "Invalid data type structure. ";

                    if (userDataType is null)
                        throw new JsonException(invalid + "Cannot be null.");

                    if (string.IsNullOrEmpty(userDataType.DataTypeName))
                        throw new JsonException(invalid + "\"DataTypeName\" must be set to something not empty or null. \"Properties\" must be set to a dictionary ({}, where every token therein is \"Key\": value), even if empty.");

                    userDataType.ClampValueTypes();

                    //Save the new data type here.

                    await Respond(inputMessage, "Serialization succeeded!");
                }
                catch (JsonException e)
                {
                    string tips = "\nMake sure that every opening brace `{}` has a closing brace that contains all the intended values,"
                        + " that all elements in a list are separated by a comma,"
                        + " that every element name is surrounded by quotes `\"\"` and contains none,"
                        + " and each element name set to a valid value with `:`.";
                    response = "Your JSON was invalid.```" + e.Message + "```" + tips;
                    if (response.Length <= 2000) await Respond(inputMessage, response);
                    else await Respond(inputMessage, "There was a JSON error, but it was too large to print here." + tips);
                }
            }
        }
    }
}
