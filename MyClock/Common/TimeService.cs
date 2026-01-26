using System.Windows.Threading;

namespace MyClock.Common
{
    public class TimeService
    {
        private DispatcherTimer _timer;
        private int _lastSecond = -1;

        public event EventHandler TimeChanged;

        public TimeService()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(100); // Check frequently
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var currentSecond = DateTime.Now.Second;

            // Only fire the event when the second actually changes
            if (currentSecond != _lastSecond)
            {
                _lastSecond = currentSecond;
                TimeChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
