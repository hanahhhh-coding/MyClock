using MyClock.Common;
using System.Collections.Generic;

namespace MyClock.Forms.ViewModels
{
    class FormSettingsViewModel : Notifiable
    {
        private bool _alwaysOnTop;
        private bool _showMoreTimezone;
        private string[] _timeZones;

        public FormSettingsViewModel()
        {
            _alwaysOnTop = App.Settings.AlwaysOnTop;
            _showMoreTimezone = App.Settings.ShowMoreTimeZone;
            _timeZones = (string[])App.Settings.TimeZones.Clone();
        }

        public bool AlwaysOnTop { get => _alwaysOnTop; set { _alwaysOnTop = value; OnPropertyChanged(); } }
        public bool ShowMoreTimezone { get => _showMoreTimezone; set { _showMoreTimezone = value; OnPropertyChanged(); } }

        public List<string> AllTimeZones => TimeZoneProvider.GetAllCityNames();

        public string TimeZone1 { get => _timeZones[1]; set { _timeZones[1] = value; OnPropertyChanged(); } }
        public string TimeZone2 { get => _timeZones[2]; set { _timeZones[2] = value; OnPropertyChanged(); } }
        public string TimeZone3 { get => _timeZones[3]; set { _timeZones[3] = value; OnPropertyChanged(); } }
        public string TimeZone4 { get => _timeZones[4]; set { _timeZones[4] = value; OnPropertyChanged(); } }
        public string TimeZone5 { get => _timeZones[5]; set { _timeZones[5] = value; OnPropertyChanged(); } }
        public string TimeZone6 { get => _timeZones[6]; set { _timeZones[6] = value; OnPropertyChanged(); } }

        public void Apply()
        {
            App.Settings.AlwaysOnTop = _alwaysOnTop;
            App.Settings.ShowMoreTimeZone = _showMoreTimezone;
            _timeZones.CopyTo(App.Settings.TimeZones, 0);
            App.Current.MainWindow.Topmost = _alwaysOnTop;
            ((MainWindowViewModel)App.Current.MainWindow.DataContext).Refresh();
        }
    }
}
