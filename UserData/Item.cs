namespace TheRemembererDiscordBot.UserData
{
    public class Item
    {
        private readonly ItemType _Type;
        public ItemType Type { get { return _Type; } }
        private ulong _Count;
        public ulong Count { get { return _Count; } }

        public Item(ItemType type, ulong count)
        {
            _Type = type;
            _Count = count;
        }

        public void ModifyCount(ulong amount) => _Count = Math.Clamp(_Count + amount, 0, 1000000);
    }
}
