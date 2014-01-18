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
using YAMP;

namespace Sumerics
{
	/// <summary>
	/// Interaction logic for PlotSettingsWindow.xaml
	/// </summary>
	public partial class SubPlotSettingsWindow : MetroWindow
	{
		SubPlotValue value;

        public SubPlotSettingsWindow(SubPlotValue value)
		{
			InitializeComponent();
			this.value = value;
			Loaded += WindowLoaded;
		}

		void WindowLoaded(object sender, RoutedEventArgs e)
		{
			DataContext = new SubPlotSettingsViewModel(value);
		}
	}
}
