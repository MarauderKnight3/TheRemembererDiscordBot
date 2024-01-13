namespace TheRemembererDiscordBot.UserData
{
    public class Item
    {
        private readonly ItemType _Type;
        public ItemType Type { get { return _Type; } }
        private readonly int _Count;
        public int Count { get { return _Count; } }

        public Item(ItemType type, int count)
        {
            _Type = type;
            _Count = count;
        }
    }
}
