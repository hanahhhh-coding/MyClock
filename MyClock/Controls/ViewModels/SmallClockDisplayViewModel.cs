using MyClock.Common;
using System.Diagnostics;
using System.Windows;

namespace MyClock.Controls.ViewModels
{
    internal class SmallClockDisplayViewModel : Notifiable
    {
        protected string TimeFormat => "HH:mm";
        protected string DateFormat => "MMMM dd";

        public  Visibility Visible => string.IsNullOrWhiteSpace(App.Settings.TimeZones[Id]) ? Visibility.Collapsed: Visibility.Visible;

        private int id;
        public int Id
        {
            get => id;
            set
            {
                if (id == value)
                    return;
                id = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CityDisplay));
                OnPropertyChanged(nameof(TimeZone));
                OnPropertyChanged(nameof(Visible));
                Update();
            }
        }

        private string timeDisplay;
        public string TimeDisplay
        {
            get => timeDisplay;
            set
            {
                if (timeDisplay == value)
                    return;
                timeDisplay = value;
                OnPropertyChanged();
            }
        }

        private string dateDisplay;
        public string DateDisplay
        {
            get => dateDisplay;
            set
            {
                if (dateDisplay == value)
                    return;
                dateDisplay = value;
                OnPropertyChanged();
            }
        }

        public Visibility PlusOneVisible
        {
            get
            {
                DateTime localTime = DateTime.Now;
                DateTime timeZoneTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZone);

                return localTime.Date != timeZoneTime.Date ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public string DayDiff
        {
            get
            {
                DateTime localTime = DateTime.Now;
                DateTime timeZoneTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZone);

                int dayDifference = (timeZoneTime.Date - localTime.Date).Days;

                if (dayDifference > 0)
                    return "+1";
                else if (dayDifference < 0)
                    return "-1";
                else
                    return "+1";
            }
        }

        public string CityDisplay
        {
            get => App.Settings.TimeZones[Id];
        }

        public TimeZoneInfo TimeZone
        {
            get
            {
                string cityName = App.Settings.TimeZones[Id];
                if (string.IsNullOrEmpty(cityName))
                    return TimeZoneInfo.Local;
                return TimeZoneProvider.FindTimeZoneByCityName(cityName);
            }
            set
            {
                if (TimeZone == value)
                    return;
                App.Settings.TimeZones[Id] = TimeZoneProvider.GetCityNameFromTimeZone(value);
                OnPropertyChanged();
                Update();
            }
        }

        public SmallClockDisplayViewModel()
        {
            TimeZone = TimeZoneInfo.Local;
            App.TimeService.TimeChanged += TimeService_TimeChanged;
            Update();
        }

        private void TimeService_TimeChanged(object? sender, EventArgs e)
        {
            Update();
        }

        public void Update()
        {
            Task.Run(() =>
            {
                DateTime currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZone);
                string timeDisplay = currentTime.ToString(TimeFormat);
                string dateDisplay = currentTime.ToString(DateFormat);
                App.Current.Dispatcher.Invoke(() => {
                    TimeDisplay = timeDisplay;
                    DateDisplay = dateDisplay;
                    OnPropertyChanged(nameof(PlusOneVisible));
                    OnPropertyChanged(nameof(DayDiff));
                });
            });
        }
    }
}
