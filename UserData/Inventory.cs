namespace TheRemembererDiscordBot.UserData
{
    public class Inventory
    {
        public List<Item> Items { get; set; } = new();

        public Inventory() { }

        public ulong InventoryWeight()
        {
            ulong weight = 0;
            foreach (Item item in Items)
            {
                weight += (ulong)item.StackWeight();
            }
            return weight;
        }

        public string Listing(string name) => name.PadRight(35) + InventoryWeight();
    }
}
