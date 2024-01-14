using Discord.WebSocket;
using System.Text;
using TheRemembererDiscordBot.CommandComponents;
using TheRemembererDiscordBot.UserData;

namespace TheRemembererDiscordBot.Commands
{
    public class ListItemTypes : Command
    {
        public override string CommandDescription() => "List all the item types you have created.";
        public override List<CommandArgument> CommandArguments(SocketMessage message) => new() { new("Page number", new() { 1, Math.Max(1, GetItemTypePages(SaveData.GetSaveData(message.Author.Id)).Count) }, mayBeSkipped: true), new("Search query", new(), true, true) };

        public override async Task CommandAction(SocketMessage message, List<object> args)
        {
            SaveData thisUserData = SaveData.GetSaveData(message.Author.Id);

            if (thisUserData.ItemTypes.Count == 0)
            {
                await Respond(message, "You have not created any item types yet.");
                return;
            }

            string searchQuery = args.Count == 2 ? args[1].ToString() ?? string.Empty : string.Empty;

            List<string> inventoryPages = GetItemTypePages(thisUserData, searchQuery);

            int pageNumber = Math.Clamp(args.Count > 0 ? (int)args[0] - 1 : 0, 0, inventoryPages.Count - 1);

            string response = "You are viewing your inventories. Page " + (pageNumber + 1) + " of " + inventoryPages.Count + (searchQuery != string.Empty ? (" (Using supplied search query)") : string.Empty);

            await Respond(message, response);
            await Respond(message, inventoryPages[pageNumber]);
        }

        public static List<string> GetItemTypePages(SaveData thisUserData, string searchQuery = "")
        {
            List<string> itemTypeNames = thisUserData.ItemTypes.Select(x => x.Name).Where(x => x.ToLower().Contains(searchQuery.ToLower())).ToList();

            itemTypeNames.Sort();

            List<string> itemTypePages = new();

            StringBuilder currentPageText;

            for (int i = 0; i < itemTypeNames.Count;)
            {
                currentPageText = new StringBuilder("```");

                for (; (i < itemTypeNames.Count) && ((currentPageText.Length + "\n" + itemTypeNames[i]).Length < 1997); i++)
                    currentPageText.Append("\n" + itemTypeNames[i]);

                currentPageText.Append("```");
                itemTypePages.Add(currentPageText.ToString());
            }

            return itemTypePages;
        }
    }
}
