using MyClock.Common;
using System;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace MyClock
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static TimeService TimeService;
        public static Settings Settings;
        private static readonly string SettingsFilePath;

        static App()
        {
            TimeService = new TimeService();

            // Get the path to settings.xml next to the executable
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string exeDirectory = Path.GetDirectoryName(exePath);
            SettingsFilePath = Path.Combine(exeDirectory, "settings.xml");

            Settings = LoadSettings();
        }

        public App()
        {
            this.Exit += App_Exit;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            SaveSettings();
        }

        private static Settings LoadSettings()
        {
            if (!File.Exists(SettingsFilePath))
            {
                return new Settings();
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                using (FileStream fileStream = new FileStream(SettingsFilePath, FileMode.Open))
                {
                    return (Settings)serializer.Deserialize(fileStream);
                }
            }
            catch (Exception ex)
            {
                // If loading fails, return a new Settings object
                System.Diagnostics.Debug.WriteLine($"Failed to load settings: {ex.Message}");
                return new Settings();
            }
        }

        private static void SaveSettings()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                using (FileStream fileStream = new FileStream(SettingsFilePath, FileMode.Create))
                {
                    serializer.Serialize(fileStream, Settings);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to save settings: {ex.Message}");
            }
        }

    }
}
