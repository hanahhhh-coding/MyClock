using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace MyClock.Common
{
    public static class TimeZoneProvider
    {
        private static readonly Dictionary<string, TimeZoneInfo> cityToTimeZoneMap = new Dictionary<string, TimeZoneInfo>();
        private static readonly List<TimeZoneInfo> allTimeZones = new List<TimeZoneInfo>() { };

        static TimeZoneProvider()
        {
            LoadTimeZonesFromJson();
        }

        private static void LoadTimeZonesFromJson()
        {
            try
            {
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cities", "Timezones.json");
                if (!File.Exists(jsonPath))
                {
                    // Fallback to system timezones if JSON file not found
                    LoadSystemTimeZones();
                    return;
                }

                string jsonContent = File.ReadAllText(jsonPath);
                var timeZoneDataList = JsonSerializer.Deserialize<List<TimeZoneData>>(jsonContent);

                if (timeZoneDataList != null)
                {
                    var systemTimeZones = TimeZoneInfo.GetSystemTimeZones();

                    foreach (var tzData in timeZoneDataList)
                    {
                        try
                        {
                            // Find timezone by matching UTC offset
                            TimeZoneInfo timeZone = FindTimeZoneByUtcOffset(systemTimeZones, tzData.offset, tzData.isdst);

                            if (timeZone != null)
                            {
                                if (!allTimeZones.Contains(timeZone))
                                {
                                    allTimeZones.Add(timeZone);
                                }

                                // Add cities from the UTC list to the map
                                if (tzData.utc != null)
                                {
                                    foreach (var cityId in tzData.utc)
                                    {
                                        string cityName = GetCityNameFromId(cityId);
                                        Debug.WriteLine(cityName);
                                        if (!cityToTimeZoneMap.ContainsKey(cityName))
                                        {
                                            cityToTimeZoneMap[cityName] = timeZone;
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {
                            // Skip timezones that can't be matched
                            continue;
                        }
                    }
                }

                // If no timezones were loaded, fall back to system timezones
                if (allTimeZones.Count == 0)
                {
                    LoadSystemTimeZones();
                }
            }
            catch
            {
                // If anything goes wrong, fall back to system timezones
                LoadSystemTimeZones();
            }
        }

        private static TimeZoneInfo FindTimeZoneByUtcOffset(System.Collections.ObjectModel.ReadOnlyCollection<TimeZoneInfo> systemTimeZones, double offsetHours, bool isDst)
        {
            // If isdst is true, the offset in JSON is DST-adjusted (e.g., EDT = -4)
            // We need to convert it back to base offset (e.g., EST = -5) by subtracting 1 hour
            // TimeZoneInfo.BaseUtcOffset is always the standard time offset
            double baseOffsetHours = isDst ? offsetHours - 1.0 : offsetHours;
            TimeSpan targetOffset = TimeSpan.FromHours(baseOffsetHours);

            // First try: Find timezone with matching base UTC offset and DST support
            foreach (var tz in systemTimeZones)
            {
                if (tz.BaseUtcOffset == targetOffset && tz.SupportsDaylightSavingTime == isDst)
                {
                    return tz;
                }
            }

            // Second try: Find timezone with matching base UTC offset (ignore DST requirement)
            foreach (var tz in systemTimeZones)
            {
                if (tz.BaseUtcOffset == targetOffset)
                {
                    return tz;
                }
            }

            return null;
        }

        private static void LoadSystemTimeZones()
        {
            allTimeZones.Clear();
            cityToTimeZoneMap.Clear();

            foreach (var timeZone in TimeZoneInfo.GetSystemTimeZones())
            {
                allTimeZones.Add(timeZone);
                string cityName = GetCityNameFromTimeZone(timeZone);
                if (!cityToTimeZoneMap.ContainsKey(cityName))
                {
                    cityToTimeZoneMap[cityName] = timeZone;
                }
            }
        }

        private static string GetCityNameFromId(string timezoneId)
        {
            // Extract city name from timezone ID (e.g., "America/New_York" -> "New York")
            if (timezoneId.Contains("/"))
            {
                string cityPart = timezoneId.Substring(timezoneId.LastIndexOf('/') + 1);
                return cityPart.Replace('_', ' ');
            }

            return timezoneId.Replace('_', ' ');
        }

        public static List<TimeZoneInfo> GetAllTimeZones()
        {
            return allTimeZones.ToList();
        }

        public static List<string> GetAllCityNames()
        {
            var result = new List<string>();
            result.Add(string.Empty);
            var add = cityToTimeZoneMap.Keys.OrderBy(c => c).ToList();
            foreach (var item in add)
            {
                result.Add(item);
            }
            return result;
        }

        public static string GetCityNameFromTimeZone(TimeZoneInfo timeZone)
        {
            string id = timeZone.Id;

            // Extract city name from timezone ID (e.g., "America/New_York" -> "New York")
            if (id.Contains("/"))
            {
                string cityPart = id.Substring(id.LastIndexOf('/') + 1);
                return cityPart.Replace('_', ' ');
            }

            // For timezone IDs without slashes, use the display name or ID
            return timeZone.DisplayName.Contains("(")
                ? timeZone.DisplayName.Substring(0, timeZone.DisplayName.IndexOf("(")).Trim()
                : id.Replace('_', ' ');
        }

        public static TimeZoneInfo FindTimeZoneByCityName(string cityName)
        {
            if (string.IsNullOrWhiteSpace(cityName))
                return TimeZoneInfo.Local;

            if (cityToTimeZoneMap.TryGetValue(cityName, out TimeZoneInfo timeZone))
            {
                return timeZone;
            }

            // Fallback: try to find by display name or ID match
            foreach (var tz in TimeZoneInfo.GetSystemTimeZones())
            {
                if (GetCityNameFromTimeZone(tz).Equals(cityName, StringComparison.OrdinalIgnoreCase))
                {
                    return tz;
                }
            }

            return TimeZoneInfo.Local;
        }
    }
}
