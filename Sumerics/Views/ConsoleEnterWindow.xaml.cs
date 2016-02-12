namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using System;
    using System.Windows;
    using System.Windows.Input;

	/// <summary>
	/// Interaction logic for ConsoleEnter.xaml
	/// </summary>
	public partial class ConsoleEnterWindow : MetroWindow
	{
        readonly IConsole _console;

		public ConsoleEnterWindow(IConsole console)
		{
            _console = console;
			InitializeComponent();
			//Owner = App.Window;
			Input.Focus();
		}

		void EvaluateClick(Object sender, RoutedEventArgs e)
		{
            if (!String.IsNullOrEmpty(Input.Text))
            {
                var query = Input.Text;
                Input.Text = String.Empty;
                _console.Execute(query);
            }

            Close();
		}

		void TextBoxEnter(Object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter && e.KeyboardDevice.Modifiers == ModifierKeys.None)
			{
				EvaluateClick(sender, e);
				e.Handled = true;
			}
            else if (e.Key == Key.Escape)
            {
                Close();
            }
		}
	}
}
