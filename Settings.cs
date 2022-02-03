using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProcessMonitor
{
    internal class SettingsFormat
    {
        public int PingFrequency { get; set; }
        public bool AlertOnEmpty { get; set; }
        
        [JsonIgnore]
        public HashSet<string> Processes { get; set; }

        public SettingsFormat ()
        {
            this.PingFrequency = 5;
            this.AlertOnEmpty = false;
            this.Processes = new HashSet<string>();
        }
    }

    internal class Settings 
    {
        public SettingsFormat Values { get; set; }

        public Settings() 
        {
            if (!File.Exists(Settings.GetSettingsPath()))
            {
                string jsonWrite = JsonSerializer.Serialize(new SettingsFormat());
                File.WriteAllText(Settings.GetSettingsPath(), jsonWrite);
            }

            string jsonRead = File.ReadAllText(Settings.GetSettingsPath());
            var jsonParsed = JsonSerializer.Deserialize<SettingsFormat>(jsonRead);

            // Realistically should never happen.
            if(jsonParsed == null) throw new Exception("Something has gone horribly wrong with reading JSON settings.");
            
            this.Values = jsonParsed;
        }

        public void Save()
        { 
            string jsonWrite = JsonSerializer.Serialize(this.Values);
            File.WriteAllText(Settings.GetSettingsPath(), jsonWrite);
        }
        
        public void Change(string setting, string value)
        {
            var field = this.Values.GetType().GetProperty(setting);
            if (field == null) 
            { 
                Console.WriteLine("Unknown Setting.");
                return;
            }

            var fieldValue = field.GetValue(this.Values);
            if (fieldValue == null) 
            { 
                Console.WriteLine("Unknown field was attempted to be changed.");
                return;
            }

            var newValue = Convert.ChangeType(value, fieldValue.GetType());
            field.SetValue(this.Values, newValue);
            this.Save();

            Console.WriteLine($"Updated. { Environment.NewLine}Old: {JsonSerializer.Serialize(fieldValue)} New{JsonSerializer.Serialize(newValue)}");
        }

        public void Print()
        {
            var options = new JsonSerializerOptions();
            options.WriteIndented = true;

            var jsonWrite = JsonSerializer.Serialize(this.Values, options);
            Console.WriteLine(jsonWrite);
        }

        private static string GetSettingsPath() 
        { 
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).ToString() + "/process_monitor_settings.json";
        }
    }
}