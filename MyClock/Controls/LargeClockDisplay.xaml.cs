using MyClock.Controls.ViewModels;
using MyClock.Forms;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace MyClock.Controls
{
    /// <summary>
    /// Interaction logic for LargeClockDisplay.xaml
    /// </summary>
    public partial class LargeClockDisplay : UserControl
    {
        private ClockDisplayViewModel viewModel;

        public LargeClockDisplay()
        {
            this.viewModel = new ClockDisplayViewModel();
            DataContext = viewModel;
            InitializeComponent();

            // Subscribe to property changes to animate text updates
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ClockDisplayViewModel.DateDisplay))
            {
                AnimateTextChange(DateTextBlock);
            }
        }

        private void AnimateTextChange(TextBlock textBlock)
        {
            // Create a fade-out and fade-in animation at 60 fps
            var fadeAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.3,
                Duration = TimeSpan.FromMilliseconds(150),
                AutoReverse = true,
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };

            // Set the frame rate to 60 fps
            Timeline.SetDesiredFrameRate(fadeAnimation, 60);

            textBlock.BeginAnimation(UIElement.OpacityProperty, fadeAnimation);
        }

        private void LargeClockDisplay_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var formSettings = new FormSettings();
            formSettings.Owner = App.Current.MainWindow;
            formSettings.ShowDialog();
        }
    }
}
