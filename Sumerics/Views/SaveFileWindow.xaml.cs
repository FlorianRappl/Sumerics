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
    /// Interaction logic for SaveFileWindow.xaml
    /// </summary>
    public partial class SaveFileWindow : MetroWindow
    {
        #region Fields

        readonly SaveFileViewModel _vm;

        #endregion

        #region ctor

        public SaveFileWindow(SaveFileViewModel vm)
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

        #endregion

        #region Methods

        public void AddFilter(String name, String value)
        {
            _vm.AddFilter(name, value);
        }

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
            _vm.CanAccept = !tb.Text.Equals(String.Empty) && _vm.IsValid(tb.Text);
        }

        #endregion
    }
}
