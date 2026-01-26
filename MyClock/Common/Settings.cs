namespace MyClock.Common
{
    [Serializable]
    public class Settings
    {
        public bool AlwaysOnTop { get; set; }
        public bool ShowMoreTimeZone { get; set; } = true;
        public double WindowLeft { get; set; } = double.NaN;
        public double WindowTop { get; set; } = double.NaN;

        public string[] TimeZones { get; set; } = new string[7] 
        {
            "", 
            "Seattle",
            "Beijing",
            "Tokyo",
            "London",
            "Berlin",
            "",
        };
    }
}
