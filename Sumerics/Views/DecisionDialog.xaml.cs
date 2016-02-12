namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using System;
    using System.Windows;
    using System.Windows.Input;

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

        public Int32 UserDecision
        {
            get { return Input.SelectedIndex; }
            set { Input.SelectedIndex = value; }
        }

        public String UserMessage
        {
            get { return Message.Text; }
            set { Message.Text = value; }
        }

        public static Int32 Show(String message, String[] decisions)
        {
            var inp = new DecisionDialog();
            inp.Message.Text = string.IsNullOrEmpty(message) ? "Your decision is required:" : message;
            inp.Input.ItemsSource = decisions != null && decisions.Length > 0 ? decisions : new [] { "The only choice." };
            inp.Input.SelectedIndex = 0;
            inp.ShowDialog();
            return inp.UserDecision;
        }

        void CloseClick(Object sender, RoutedEventArgs e)
        {
            Close();
        }

        void TextBoxEnter(Object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && e.KeyboardDevice.Modifiers == ModifierKeys.None)
            {
                CloseClick(sender, e);
            }
            else if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
