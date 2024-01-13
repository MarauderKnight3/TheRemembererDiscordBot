using System.Text.Json;

namespace TheRemembererDiscordBot.UserData
{
    public class SaveData
    {
        public List<ItemType> ItemTypes { get; set; } = new();
        public Dictionary<string, Inventory> Inventories { get; set; } = new();
        public SaveData() { }
        public void Save(ulong userID) => Write(userID, this);

        public static readonly DirectoryInfo SaveDirectory = Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\SaveData");
        private static readonly Dictionary<ulong, SaveData> LoadedData = new();
        public static string Read(ulong userID) => SaveDirectory.GetFiles(userID + ".json").Any() ? File.ReadAllText(SaveDirectory.GetFiles(userID + ".json").ElementAt(0).FullName) : string.Empty;
        public static void Write(ulong userID, SaveData dataToSave) => File.WriteAllText(SaveDirectory.FullName + @"\" + userID + ".json", JsonSerializer.Serialize(dataToSave));

        public static SaveData GetSaveData(ulong userID)
        {
            if (LoadedData.ContainsKey(userID))
                return LoadedData[userID];

            string json = Read(userID);
            SaveData save;

            if (json == string.Empty)
            {
                save = new SaveData();
            }
            else
                save = JsonSerializer.Deserialize<SaveData>(json) ?? throw new Exception();

            LoadedData.Add(userID, save);

            return save;
        }
    }
}
