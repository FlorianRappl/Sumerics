namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using Sumerics.ViewModels;
    using YAMP;

	/// <summary>
	/// Interaction logic for PlotSeriesWindow.xaml
	/// </summary>
	public partial class PlotSeriesWindow : MetroWindow
	{
		public PlotSeriesWindow(XYPlotValue value)
		{
			InitializeComponent();
            DataContext = new SeriesViewModel(value);
		}
	}
}
