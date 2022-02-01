using System;
using System.IO;
using System.Text.Json;

namespace ProcessMonitor
{
    internal class SettingsFormat
    {
        public int PingFrequency { get; set; }
        public HashSet<string> Processes { get; set; }

        public SettingsFormat ()
        {
            this.PingFrequency = 5;
            this.Processes = new HashSet<string>();
        }
    }

    internal class Settings 
    {
        public SettingsFormat settings { get; set; }

        public Settings() 
        {
            if (!File.Exists(Settings.GetSettingsPath()))
            {
                string jsonWrite = JsonSerializer.Serialize(new SettingsFormat());
                File.WriteAllText(Settings.GetSettingsPath(), jsonWrite);
            }

            string jsonRead = File.ReadAllText(Settings.GetSettingsPath());
            this.settings = JsonSerializer.Deserialize<SettingsFormat>(jsonRead);
        }

        public void Save()
        { 
            string jsonWrite = JsonSerializer.Serialize(this.settings);
            File.WriteAllText(Settings.GetSettingsPath(), jsonWrite);
        }

        public static string GetSettingsPath() 
        { 
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).ToString() + "/process_monitor_settings.json";
        }
    }
}