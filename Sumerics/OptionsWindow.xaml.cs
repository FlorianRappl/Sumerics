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
	/// Interaction logic for OptionsWindow.xaml
	/// </summary>
	public partial class OptionsWindow : MetroWindow
	{
		public OptionsWindow()
		{
			InitializeComponent();
			Loaded += OptionsWindowLoaded;
		}

		private void OptionsWindowLoaded(object sender, RoutedEventArgs e)
		{
			DataContext = new OptionsViewModel();
		}
	}
}
