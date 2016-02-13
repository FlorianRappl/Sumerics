namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for OutputDialog.xaml
    /// </summary>
    public partial class OutputDialog : MetroWindow
    {
        public OutputDialog()
        {
            InitializeComponent();
        }

        void CloseClick(Object sender, RoutedEventArgs e)
        {
            Close();
        }

        public static void Show(String title, String message)
        {
            var outp = new OutputDialog();
            outp.Title = title;
            outp.Output.Text = message;
            outp.ShowDialog();
        }
    }
}
