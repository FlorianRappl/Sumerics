using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace Sumerics
{
	/// <summary>
	/// Interaction logic for FolderBrowseWindow.xaml
	/// </summary>
	public partial class FolderBrowseWindow : MetroWindow
    {
        #region Members

        FolderBrowseViewModel model;

        #endregion

        #region ctor

        public FolderBrowseWindow()
        {
            model = new FolderBrowseViewModel(Environment.CurrentDirectory);
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
