namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using Sumerics.ViewModels;
    using YAMP;

	/// <summary>
	/// Interaction logic for PlotSeriesWindow.xaml
	/// </summary>
	public partial class HeatSeriesWindow : MetroWindow
	{
        public HeatSeriesWindow(HeatmapPlotValue value)
		{
            InitializeComponent();
            DataContext = new HeatmapViewModel(value);
		}
	}
}
