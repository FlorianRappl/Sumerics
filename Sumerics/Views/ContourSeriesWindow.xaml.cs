namespace Sumerics
{
    using MahApps.Metro.Controls;
    using YAMP;

	/// <summary>
	/// Interaction logic for PlotSeriesWindow.xaml
	/// </summary>
	public partial class ContourSeriesWindow : MetroWindow
	{
		public ContourSeriesWindow(ContourPlotValue value, IContainer container)
		{
            InitializeComponent();
            DataContext = new ContourViewModel(value, container);
		}
	}
}
