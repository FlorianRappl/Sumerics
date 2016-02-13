namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using System;
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

		void CloseCommandHandler(Object sender, ExecutedRoutedEventArgs e)
		{
			Close();
		}

		void ContentLoaded(Object sender, NavigationEventArgs e)
		{
			Loading.Visibility = System.Windows.Visibility.Hidden;
			Browser.Visibility = System.Windows.Visibility.Visible;
		}
	}
}
