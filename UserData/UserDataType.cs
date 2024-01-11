using System.Text.Json;
using System.Text.Json.Serialization;

namespace TheRemembererDiscordBot.UserData
{
    public class UserDataType
    {
        public static readonly string Examples = string.Concat(new List<UserDataType>()
            {
                new("Example Data Type", new() {
                    { "This is the name for a text value, and it is signified as a text value with the code 0", AllowedValueTypes.TextValue },
                    { "This is the name for a number, and it is signified as a number with the code 1", AllowedValueTypes.DecimalValue }
                }),
                new("Item Type", new() {
                    { "Name", AllowedValueTypes.TextValue },
                    { "Weight", AllowedValueTypes.DecimalValue }
                })
            }.Select(x => "```json\n" + JsonSerializer.Serialize(x, options: new()
            {
                WriteIndented = true
            }) + "```"));

        private readonly string _DataTypeName;
        public string DataTypeName { get { return _DataTypeName; } }

        private readonly Dictionary<string, AllowedValueTypes> _Properties = new();
        public Dictionary<string, AllowedValueTypes> Properties { get { return _Properties; } }

        public enum AllowedValueTypes
        {
            TextValue,
            DecimalValue
        }

        public UserDataType(string dataTypeName)
        {
            _DataTypeName = dataTypeName;
            _Properties = new();
        }

        [JsonConstructor]
        public UserDataType(string dataTypeName, Dictionary<string, AllowedValueTypes> properties)
        {
            if (string.IsNullOrEmpty(dataTypeName) || properties is null)
            {
                _DataTypeName = "";
                _Properties = new();
            }
            else
            {
                _DataTypeName = dataTypeName;
                _Properties = properties;
            }
        }

        public void AddProperty(string propertyName, AllowedValueTypes propertyType) => _Properties.Add(propertyName, propertyType);
        public void RemoveProperty(string propertyName) => _Properties.Remove(propertyName);

        public UserDataType ClampValueTypes()
        {
            for (int i = 0; i < _Properties.Count; i++)
            {
                string current = _Properties.ElementAt(i).Key;

                if (_Properties[current] is not AllowedValueTypes.DecimalValue)
                {
                    _Properties[current] = AllowedValueTypes.TextValue;
                }
            }

            return this;
        }
    }
}
