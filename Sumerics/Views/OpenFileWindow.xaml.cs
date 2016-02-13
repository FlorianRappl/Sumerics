namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using Sumerics.Models;
    using Sumerics.ViewModels;
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for OpenFileWindow.xaml
    /// </summary>
    public partial class OpenFileWindow : MetroWindow
    {
        #region Fields

        readonly OpenFileViewModel _vm;

        #endregion

        #region ctor

        public OpenFileWindow(OpenFileViewModel vm)
        {
            InitializeComponent();
            DataContext = _vm = vm;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value if the dialog was accepted, i.e.
        /// some file got picked.
        /// </summary>
        public Boolean Accepted
        {
            get { return _vm.Accepted; }
        }

        /// <summary>
        /// Gets or sets the path to the currently selected file.
        /// </summary>
        public string SelectedFile
        {
            get { return _vm.SelectedFile.FullName; }
            set { _vm.SelectedFile = new FileModel(value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a filer to the dialog.
        /// </summary>
        /// <param name="name">The description.</param>
        /// <param name="value">The value of the filter.</param>
        public void AddFilter(String name, String value)
        {
            _vm.AddFilter(name, value);
        }

        /// <summary>
        /// Removes a filter from the dialog.
        /// </summary>
        /// <param name="name">The description.</param>
        public void RemoveFilter(String name)
        {
            _vm.RemoveFilter(name);
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

                if (System.IO.Directory.Exists(path))
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
            _vm.CanAccept = System.IO.File.Exists(_vm.CurrentDirectory.FullName + "\\" + tb.Text);
        }

        #endregion
    }
}
