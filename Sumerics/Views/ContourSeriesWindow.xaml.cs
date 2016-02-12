namespace Sumerics
{
    using MahApps.Metro.Controls;
    using YAMP;

	/// <summary>
	/// Interaction logic for PlotSeriesWindow.xaml
	/// </summary>
	public partial class ContourSeriesWindow : MetroWindow
	{
		public ContourSeriesWindow(ContourPlotValue value)
		{
            InitializeComponent();
            DataContext = new ContourViewModel(value);
		}
	}
}
