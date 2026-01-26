using System.Collections.Generic;

namespace MyClock.Common
{
    public class TimeZoneData
    {
        public string value { get; set; }
        public string abbr { get; set; }
        public double offset { get; set; }
        public bool isdst { get; set; }
        public string text { get; set; }
        public List<string> utc { get; set; }
    }
}
