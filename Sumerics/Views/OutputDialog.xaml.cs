namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
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

        void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public static void Show(string title, string message)
        {
            var outp = new OutputDialog();
            outp.Title = title;
            outp.Output.Text = message;
            outp.ShowDialog();
        }
    }
}
