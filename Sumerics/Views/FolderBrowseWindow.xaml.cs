namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using Sumerics.Models;
    using Sumerics.ViewModels;
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;

	/// <summary>
	/// Interaction logic for FolderBrowseWindow.xaml
	/// </summary>
	public partial class FolderBrowseWindow : MetroWindow
    {
        #region Fields

        readonly FolderBrowseViewModel _vm;

        #endregion

        #region ctor

        public FolderBrowseWindow(FolderBrowseViewModel vm)
        {
			InitializeComponent();
            DataContext = _vm = vm;
		}

        #endregion

        #region Properties

        public Boolean Accepted
        {
            get { return _vm.Accepted; }
        }

        public String SelectedDirectory
        {
            get { return _vm.SelectedDirectory.FullName; }
            set { _vm.SelectedDirectory = new FolderModel(value); }
        }

        #endregion

        #region Events

        async void ClearSelected(Object sender, RoutedEventArgs e)
        {
            var view = sender as ListView;

            if (view != null)
            {
                view.SelectedIndex = -1;
                //This hack seems strange but unfortunately it is the way to go
                await Task.Delay(10);
                Current.Focus();
            }
        }

        #endregion
    }
}
