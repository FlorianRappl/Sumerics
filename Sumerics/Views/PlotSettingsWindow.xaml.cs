namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using Sumerics.ViewModels;

    /// <summary>
	/// Interaction logic for PlotSettingsWindow.xaml
	/// </summary>
	public partial class PlotSettingsWindow : MetroWindow
	{
        public PlotSettingsWindow(PlotSettingsViewModel vm)
		{
			InitializeComponent();
            DataContext = vm;
		}
	}
}
