using MyClock.Common;
using System.Diagnostics;
using System.Windows;

namespace MyClock.Controls.ViewModels
{
    internal class ClockDisplayViewModel : Notifiable
    {
        protected virtual string TimeFormat { get => "HH:mm:ss"; }
        protected virtual string DateFormat { get => "dddd, MMMM dd, yyyy"; }

        public virtual Visibility Visible { get => Visibility.Visible; }

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

        public TimeZoneInfo TimeZone
        {
            get
            {
                return TimeZoneInfo.Local;
            }
        }

        private int lastSecond = -1;

        public ClockDisplayViewModel()
        {
            App.TimeService.TimeChanged += TimeService_TimeChanged;
            Update();
        }

        private void TimeService_TimeChanged(object? sender, EventArgs e)
        {
            Update();
        }

        public void Update()
        {
            DateTime currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZone);

            // Only update if the second has actually changed
            if (currentTime.Second != lastSecond)
            {
                lastSecond = currentTime.Second;
                TimeDisplay = currentTime.ToString(TimeFormat);
                DateDisplay = currentTime.ToString(DateFormat);
            }
        }
    }
}
