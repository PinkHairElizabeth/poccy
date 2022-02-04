using System.Text.Json.Serialization;

namespace Poccy
{
    internal class SettingsFormat
    {
        public int PingFrequency { get; set; }
        public bool AlertOnEmpty { get; set; }

        [JsonIgnore]
        public HashSet<string> Processes { get; set; }

        public SettingsFormat()
        {
            this.PingFrequency = 5;
            this.AlertOnEmpty = false;
            this.Processes = new HashSet<string>();
        }
    }
}