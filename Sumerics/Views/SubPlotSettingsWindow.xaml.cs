namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using Sumerics.ViewModels;
    using YAMP;

	/// <summary>
	/// Interaction logic for PlotSettingsWindow.xaml
	/// </summary>
	public partial class SubPlotSettingsWindow : MetroWindow
	{
        public SubPlotSettingsWindow(SubPlotValue value)
		{
            InitializeComponent();
            DataContext = new SubPlotSettingsViewModel(value);
		}
	}
}
