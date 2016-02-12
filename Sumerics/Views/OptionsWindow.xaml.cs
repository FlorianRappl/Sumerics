namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using Sumerics.ViewModels;

    /// <summary>
	/// Interaction logic for OptionsWindow.xaml
	/// </summary>
	public partial class OptionsWindow : MetroWindow
	{
		public OptionsWindow(IApplication app)
		{
            InitializeComponent();
            DataContext = new OptionsViewModel(app.Settings);
		}
	}
}
