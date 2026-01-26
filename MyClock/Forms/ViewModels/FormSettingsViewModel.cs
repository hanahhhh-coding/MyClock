using MyClock.Common;
using System;
using System.Collections.Generic;

namespace MyClock.Forms.ViewModels
{
    class FormSettingsViewModel : Notifiable
    {
        public bool AlwaysOnTop { get => App.Settings.AlwaysOnTop; set => App.Settings.AlwaysOnTop = value; }
        public bool ShowMoreTimezone { get => App.Settings.ShowMoreTimeZone; set => App.Settings.ShowMoreTimeZone = value; }

        public List<string> AllTimeZones => TimeZoneProvider.GetAllCityNames();

        public string TimeZone1
        {
            get => App.Settings.TimeZones[1];
            set
            {
                App.Settings.TimeZones[1] = value;
                OnPropertyChanged();
            }
        }

        public string TimeZone2
        {
            get => App.Settings.TimeZones[2];
            set
            {
                App.Settings.TimeZones[2] = value;
                OnPropertyChanged();
            }
        }

        public string TimeZone3
        {
            get => App.Settings.TimeZones[3];
            set
            {
                App.Settings.TimeZones[3] = value;
                OnPropertyChanged();
            }
        }

        public string TimeZone4
        {
            get => App.Settings.TimeZones[4];
            set
            {
                App.Settings.TimeZones[4] = value;
                OnPropertyChanged();
            }
        }

        public string TimeZone5
        {
            get => App.Settings.TimeZones[5];
            set
            {
                App.Settings.TimeZones[5] = value;
                OnPropertyChanged();
            }
        }

        public string TimeZone6
        {
            get => App.Settings.TimeZones[6];
            set
            {
                App.Settings.TimeZones[6] = value;
                OnPropertyChanged();
            }
        }
    }
}
