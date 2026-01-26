using MyClock.Forms.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyClock.Forms
{
    /// <summary>
    /// Interaction logic for FormSettings.xaml
    /// </summary>
    public partial class FormSettings : Window
    {
        private FormSettingsViewModel viewModel;

        public FormSettings()
        {
            viewModel = new FormSettingsViewModel();
            DataContext = viewModel;
            InitializeComponent();
        }

        private void ComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.ItemsSource != null)
            {
                string keyChar = e.Key.ToString();

                if (keyChar.Length == 1 && char.IsLetter(keyChar[0]))
                {
                    string searchLetter = keyChar.ToUpper();

                    foreach (var item in comboBox.ItemsSource)
                    {
                        string itemText = item?.ToString() ?? "";
                        if (itemText.StartsWith(searchLetter, StringComparison.OrdinalIgnoreCase))
                        {
                            comboBox.SelectedItem = item;
                            e.Handled = true;
                            break;
                        }
                    }
                }
            }
        }
    }
}
