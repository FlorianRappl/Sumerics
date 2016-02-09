namespace Sumerics
{
    using MahApps.Metro.Controls;
    using YAMP;

	/// <summary>
	/// Interaction logic for PlotSettingsWindow.xaml
	/// </summary>
	public partial class PlotSettingsWindow : MetroWindow
	{
		public PlotSettingsWindow(XYPlotValue value, IContainer container)
		{
			InitializeComponent();
            DataContext = new PlotSettingsViewModel(value, container);
		}
	}
}
