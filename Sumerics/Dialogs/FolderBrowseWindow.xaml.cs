namespace Sumerics
{
    using MahApps.Metro.Controls;
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;

	/// <summary>
	/// Interaction logic for FolderBrowseWindow.xaml
	/// </summary>
	public partial class FolderBrowseWindow : MetroWindow
    {
        #region Members

        FolderBrowseViewModel model;

        #endregion

        #region ctor

        public FolderBrowseWindow(IContainer container)
        {
            model = new FolderBrowseViewModel(Environment.CurrentDirectory, container);
			InitializeComponent();
            DataContext = model;
		}

        #endregion

        #region Properties

        public bool Accepted
        {
            get 
            {
                return model.Accepted;
            }
        }

        public string SelectedDirectory
        {
            get
            {
                return model.SelectedDirectory.FullName;
            }
            set
            {
                model.SelectedDirectory = new FolderModel(value);
            }
        }

        #endregion

        #region Events

        void ClearSelected(object sender, RoutedEventArgs e)
        {
            (sender as ListView).SelectedIndex = -1;
            WaitAndFocus();
        }

        async void WaitAndFocus()
        {
            //This hack seems strange but unfortunately it is the way to go
            await Task.Delay(10);
            Current.Focus();
        }

        #endregion
    }
}
