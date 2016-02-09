namespace Sumerics
{
    using MahApps.Metro.Controls;

    /// <summary>
	/// Interaction logic for OptionsWindow.xaml
	/// </summary>
	public partial class OptionsWindow : MetroWindow
	{
		public OptionsWindow(IContainer container)
		{
            InitializeComponent();
            DataContext = new OptionsViewModel(container);
		}
	}
}
