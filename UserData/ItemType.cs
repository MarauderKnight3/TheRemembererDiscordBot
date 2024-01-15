namespace TheRemembererDiscordBot.UserData
{
    public class ItemType
    {
        private string _Name;
        public string Name { get { return _Name; } }
        private int _Weight;
        public int Weight { get { return _Weight; } }

        public ItemType(string name, int weight)
        {
            _Name = name;
            _Weight = weight;
        }

        public void SetName(string name) => _Name = name;
        public void SetWeight(int weight) => _Weight = weight;
    }
}
