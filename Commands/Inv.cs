using Discord.WebSocket;
using System.Text;
using TheRemembererDiscordBot.CommandComponents;
using TheRemembererDiscordBot.UserData;

namespace TheRemembererDiscordBot.Commands
{
    public class Inv : Command
    {
        public override string CommandDescription() => "View an inventory.";
        public override List<CommandArgument> CommandArguments(SocketMessage message) => new()
        {
            new("Inventory to view", SaveData.GetSaveData(message.Author.Id).Inventories.Keys.ToList<object>()),
            new("Page number", new() { 1, 1000 }, mayBeSkipped: true),
            new("Search query", new(), true, true)
        };

        public override async Task CommandAction(SocketMessage message, List<object> args)
        {
            SaveData thisUserData = SaveData.GetSaveData(message.Author.Id);

            string targetInventoryName = args[0].ToString() ?? "";

            Inventory targetInventory = thisUserData.Inventories[targetInventoryName];

            string searchQuery = args.Count == 3 ? args[2].ToString() ?? string.Empty : string.Empty;

            List<string> itemPages = GetItemPages(targetInventory, searchQuery);

            int pageNumber = Math.Clamp(args.Count > 1 ? (int)args[1] - 1 : 0, 0, itemPages.Count - 1);

            string response = "You are viewing the " + targetInventoryName + " inventory. Page " + (pageNumber + 1) + " of " + itemPages.Count + (searchQuery != string.Empty ? (" (Using supplied search query)") : string.Empty);

            await Respond(message, response);

            await Respond(message, itemPages[pageNumber]);
        }

        public static List<string> GetItemPages(Inventory inventory, string searchQuery = "")
        {
            List<string> items = inventory.Items.Where(x => x.Type.Name.ToLower().Contains(searchQuery.ToLower())).OrderBy(x => x.Type.Name).Select(x => x.Listing()).ToList();

            List<string> itemPages = new();

            StringBuilder currentPageText;

            for (int i = 0; i < items.Count;)
            {
                currentPageText = new StringBuilder("```" + "Name (Count)".PadRight(35) + "Weight");

                for (; (i < items.Count) && ((currentPageText.Length + "\n" + items[i]).Length < 1997); i++)
                    currentPageText.Append("\n" + items[i]);

                currentPageText.Append("```");
                itemPages.Add(currentPageText.ToString());
            }

            return itemPages;
        }
    }
}
