namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using System.Windows.Input;
    using System.Windows.Navigation;

	/// <summary>
	/// Interaction logic for DemoBrowser.xaml
	/// </summary>
	public partial class DemoBrowser : MetroWindow
	{
		public DemoBrowser()
		{
			InitializeComponent();
		}

		void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
		{
			Close();
		}

		private void ContentLoaded(object sender, NavigationEventArgs e)
		{
			Loading.Visibility = System.Windows.Visibility.Hidden;
			Browser.Visibility = System.Windows.Visibility.Visible;
		}
	}
}
