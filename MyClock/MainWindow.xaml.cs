using MyClock.Forms;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace MyClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel;

        // Windows API for title bar customization
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int cx, int cy, uint flags);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_CAPTION_COLOR = 35;

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_DLGMODALFRAME = 0x0001;
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_FRAMECHANGED = 0x0020;

        public MainWindow()
        {
            viewModel = new MainWindowViewModel();
            DataContext = viewModel;
            InitializeComponent();

            // Set title bar to black
            SetTitleBarColor();

            // Set MinHeight based on whether more timezones are shown
            if (App.Settings.ShowMoreTimeZone)
            {
                MinHeight = 330;
            }
            else
            {
                MinHeight = 170;
            }

            // Restore window position if saved and within screen bounds
            if (!double.IsNaN(App.Settings.WindowLeft) && !double.IsNaN(App.Settings.WindowTop))
            {
                if (IsPositionOnScreen(App.Settings.WindowLeft, App.Settings.WindowTop))
                {
                    this.Left = App.Settings.WindowLeft;
                    this.Top = App.Settings.WindowTop;
                }
            }

            // Save window position when closing
            this.Closing += MainWindow_Closing;
        }

        private bool IsPositionOnScreen(double left, double top)
        {
            // Get the virtual screen dimensions
            double virtualScreenLeft = SystemParameters.VirtualScreenLeft;
            double virtualScreenTop = SystemParameters.VirtualScreenTop;
            double virtualScreenWidth = SystemParameters.VirtualScreenWidth;
            double virtualScreenHeight = SystemParameters.VirtualScreenHeight;

            // Check if the position is within the virtual screen bounds
            // Allow at least 100 pixels of the window to be visible
            const double minVisiblePixels = 100;

            return left + minVisiblePixels >= virtualScreenLeft &&
                   left < virtualScreenLeft + virtualScreenWidth &&
                   top + minVisiblePixels >= virtualScreenTop &&
                   top < virtualScreenTop + virtualScreenHeight;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Settings.WindowLeft = this.Left;
            App.Settings.WindowTop = this.Top;
        }

        private void SetTitleBarColor()
        {
            IntPtr hwnd = new WindowInteropHelper(this).EnsureHandle();

            // Set dark mode for title bar
            int useDarkMode = 1;
            DwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE, ref useDarkMode, sizeof(int));

            // Set caption color to black (0x00000000 = black in BGR format)
            int captionColor = 0x00000000;
            DwmSetWindowAttribute(hwnd, DWMWA_CAPTION_COLOR, ref captionColor, sizeof(int));

            // Hide the icon on title bar
            int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_DLGMODALFRAME);
            SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
        }
    }
}