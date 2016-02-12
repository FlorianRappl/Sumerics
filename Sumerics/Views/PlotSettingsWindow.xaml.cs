namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using Sumerics.ViewModels;
    using YAMP;

	/// <summary>
	/// Interaction logic for PlotSettingsWindow.xaml
	/// </summary>
	public partial class PlotSettingsWindow : MetroWindow
	{
		public PlotSettingsWindow(XYPlotValue value)
		{
			InitializeComponent();
            DataContext = new PlotSettingsViewModel(value);
		}
	}
}
