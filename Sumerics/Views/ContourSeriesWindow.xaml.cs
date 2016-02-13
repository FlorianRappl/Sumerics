namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using Sumerics.ViewModels;

    /// <summary>
	/// Interaction logic for PlotSeriesWindow.xaml
	/// </summary>
	public partial class ContourSeriesWindow : MetroWindow
	{
        public ContourSeriesWindow(ContourViewModel vm)
		{
            InitializeComponent();
            DataContext = vm;
		}
	}
}
