namespace Sumerics
{
    using MahApps.Metro.Controls;
    using YAMP;

	/// <summary>
	/// Interaction logic for PlotSeriesWindow.xaml
	/// </summary>
	public partial class HeatSeriesWindow : MetroWindow
	{
        public HeatSeriesWindow(HeatmapPlotValue value, IContainer container)
		{
            InitializeComponent();
            DataContext = new HeatmapViewModel(value, container);
		}
	}
}
