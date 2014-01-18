using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sumerics
{
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
