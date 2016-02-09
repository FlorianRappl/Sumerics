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
    /// Interaction logic for DecisionDialog.xaml
    /// </summary>
    public partial class DecisionDialog : MetroWindow
    {
        public DecisionDialog()
        {
            InitializeComponent();
            Input.Focus();
        }

        public int UserDecision
        {
            get { return Input.SelectedIndex; }
            set { Input.SelectedIndex = value; }
        }

        public string UserMessage
        {
            get { return Message.Text; }
            set { Message.Text = value; }
        }

        public static int Show(string message, string[] decisions)
        {
            var inp = new DecisionDialog();
            inp.Message.Text = string.IsNullOrEmpty(message) ? "Your decision is required:" : message;
            inp.Input.ItemsSource = decisions != null && decisions.Length > 0 ? decisions : new [] { "The only choice." };
            inp.Input.SelectedIndex = 0;
            inp.ShowDialog();
            return inp.UserDecision;
        }

        void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        void TextBoxEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && e.KeyboardDevice.Modifiers == ModifierKeys.None)
                CloseClick(sender, e);
            else if (e.Key == Key.Escape)
                Close();
        }
    }
}
