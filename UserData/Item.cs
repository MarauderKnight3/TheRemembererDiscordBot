namespace TheRemembererDiscordBot.UserData
{
    public class Item
    {
        private ItemType _Type;
        public ItemType Type { get { return _Type; } }
        private long _Count;
        public long Count { get { return _Count; } }

        public Item(ItemType type, long count)
        {
            _Type = type;
            _Count = count;
        }

        public void ChangeType(ItemType newType) => _Type = newType;

        public void ModifyCount(long amount) => _Count = Math.Clamp(_Count + amount, 0, 1000000);

        public int StackWeight() => Type.Weight * (int)Count;

        public string Listing() => (Type.Name + " (" + Count + ")").PadRight(35) + StackWeight();
    }
}
