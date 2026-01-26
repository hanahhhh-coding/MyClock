using MyClock.Common;
using System.Windows;

namespace MyClock
{
    internal class MainWindowViewModel : Notifiable
    {
        public Visibility MoreTimeVisibility { get => App.Settings.ShowMoreTimeZone ? Visibility.Visible : Visibility.Collapsed; }
        public bool AlwaysOnTop { get => App.Settings.AlwaysOnTop; }
    }
}
