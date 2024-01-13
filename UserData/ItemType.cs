﻿namespace TheRemembererDiscordBot.UserData
{
    public class ItemType
    {
        private readonly string _Name;
        public string Name { get { return _Name; } }
        private readonly decimal _Weight;
        public decimal Weight { get { return _Weight; } }

        public ItemType(string name, decimal weight)
        {
            _Name = name;
            _Weight = weight;
        }
    }
}
