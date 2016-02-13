namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using Sumerics.Models;
    using Sumerics.ViewModels;
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for SaveFileWindow.xaml
    /// </summary>
    public partial class SaveImageWindow : MetroWindow
    {
        #region Fields

        readonly SaveImageViewModel _vm;

        #endregion

        #region ctor

        public SaveImageWindow(SaveImageViewModel vm)
        {
            InitializeComponent();
            DataContext = _vm = vm;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the status if the dialog has been accepted.
        /// </summary>
        public Boolean Accepted
        {
            get { return _vm.Accepted; }
        }

        /// <summary>
        /// Gets or sets the currently selected file.
        /// </summary>
        public String SelectedFile
        {
            get { return _vm.UserSelectedFile.FullName; }
            set { _vm.FileName = value; }
        }

        /// <summary>
        /// Gets or sets currently selected the image width.
        /// </summary>
        public Int32 ImageWidth
        {
            get { return _vm.ImageWidth; }
            set { _vm.ImageWidth = value; }
        }

        /// <summary>
        /// Gets or sets the currently selected image height.
        /// </summary>
        public Int32 ImageHeight
        {
            get { return _vm.ImageHeight; }
            set { _vm.ImageHeight = value; }
        }

        #endregion

        #region Events

        async void ClearSelected(Object sender, RoutedEventArgs e)
        {
            var view = sender as ListView;

            if (view != null)
            {
                view.SelectedIndex = -1;

                if (sender != Current)
                {
                    //This hack seems strange but unfortunately it is the way to go
                    await Task.Delay(10);
                    Current.Focus();
                }
            }
            
        }

        void TextBoxKeyPressed(Object sender, KeyEventArgs e)
        {
            var tb = sender as TextBox;

            if (e.Key == Key.Enter)
            {
                _vm.FileName = tb.Text;
                var path = _vm.CurrentDirectory.FullName + "\\" + _vm.FileName;

                if (Directory.Exists(path))
                {
                    _vm.CurrentDirectory = new FolderModel(path);
                }
                else if (_vm.CanAccept)
                {
                    _vm.Accept.Execute(this);
                }
            }
            else if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        void TextBoxChanged(Object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            _vm.CanAccept = !tb.Text.Equals(String.Empty) && _vm.IsValid(tb.Text);
        }

        #endregion
    }
}