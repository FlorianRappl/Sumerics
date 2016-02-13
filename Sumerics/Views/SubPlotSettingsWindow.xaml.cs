namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using Sumerics.ViewModels;

    /// <summary>
	/// Interaction logic for PlotSettingsWindow.xaml
	/// </summary>
	public partial class SubPlotSettingsWindow : MetroWindow
	{
        public SubPlotSettingsWindow(SubPlotSettingsViewModel vm)
		{
            InitializeComponent();
            DataContext = vm;
		}
	}
}
