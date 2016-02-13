namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using Sumerics.ViewModels;

    /// <summary>
	/// Interaction logic for PlotSeriesWindow.xaml
	/// </summary>
	public partial class PlotSeriesWindow : MetroWindow
	{
        public PlotSeriesWindow(SeriesViewModel vm)
		{
			InitializeComponent();
            DataContext = vm;
		}
	}
}
