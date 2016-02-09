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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace Sumerics
{
	/// <summary>
	/// Interaction logic for DemoBrowser.xaml
	/// </summary>
	public partial class DemoBrowser : MetroWindow
	{
		public DemoBrowser()
		{
			InitializeComponent();
		}

		void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
		{
			Close();
		}

		private void ContentLoaded(object sender, NavigationEventArgs e)
		{
			Loading.Visibility = System.Windows.Visibility.Hidden;
			Browser.Visibility = System.Windows.Visibility.Visible;
		}
	}
}
