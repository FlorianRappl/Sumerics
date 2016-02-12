namespace Sumerics
{
    using MahApps.Metro.Controls;

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
