namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

	/// <summary>
	/// Interaction logic for ConsoleEnter.xaml
	/// </summary>
	public partial class ConsoleEnterWindow : MetroWindow
	{
		public ConsoleEnterWindow()
		{
			InitializeComponent();
			Owner = App.Current.MainWindow;
            InputBox.Focus();
		}

		void TextBoxEnter(Object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter && e.KeyboardDevice.Modifiers == ModifierKeys.None)
			{
                var command = OkButton.Command;
                var parameter = OkButton.CommandParameter;

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
