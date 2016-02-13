namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using Sumerics.ViewModels;

    /// <summary>
	/// Interaction logic for PlotSeriesWindow.xaml
	/// </summary>
	public partial class HeatSeriesWindow : MetroWindow
	{
        public HeatSeriesWindow(HeatmapViewModel vm)
		{
            InitializeComponent();
            DataContext = vm;
		}
	}
}
