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
using MahApps.Metro.Controls;

namespace Sumerics
{
	/// <summary>
	/// Interaction logic for ConsoleEnter.xaml
	/// </summary>
	public partial class ConsoleEnterWindow : MetroWindow
	{
		public ConsoleEnterWindow()
		{
			InitializeComponent();
			Owner = App.Window;
			Input.Focus();
		}

		void EvaluateClick(object sender, RoutedEventArgs e)
		{
            if (!Input.Text.Equals(string.Empty))
            {
                string query = Input.Text;
                Input.Text = string.Empty;
                App.Window.RunQuery(query);
            }

            Close();
		}

		void TextBoxEnter(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter && e.KeyboardDevice.Modifiers == ModifierKeys.None)
			{
				EvaluateClick(sender, e);
				e.Handled = true;
			}
			else if (e.Key == Key.Escape)
				Close();
		}
	}
}
