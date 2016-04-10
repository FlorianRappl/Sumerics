namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class InputDialog : MetroWindow
    {
        public InputDialog()
        {
            InitializeComponent();
            Input.Focus();
        }

        void TextBoxEnter(Object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && e.KeyboardDevice.Modifiers == ModifierKeys.None)
            {
                var command = ConfirmButton.Command;
                var parameter = ConfirmButton.CommandParameter;

                if (command != null && command.CanExecute(parameter))
                {
                    command.Execute(parameter);
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
