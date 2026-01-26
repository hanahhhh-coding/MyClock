using MyClock.Controls.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace MyClock.Controls
{
    /// <summary>
    /// Interaction logic for SmallClockControl.xaml
    /// </summary>
    public partial class SmallClockControl : UserControl
    {
        private SmallClockDisplayViewModel viewModel;

        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register(nameof(Id), typeof(int), typeof(SmallClockControl), new PropertyMetadata(0, OnIdChanged));

        private static void OnIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SmallClockControl control)
            {
                control.viewModel.Id = (int)e.NewValue;
            }
        }

        public SmallClockControl()
        {
            viewModel = new SmallClockDisplayViewModel();
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
