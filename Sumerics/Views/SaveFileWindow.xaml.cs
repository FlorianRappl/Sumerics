namespace Sumerics
{
    using MahApps.Metro.Controls;
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
        #region Files

        readonly SaveFileViewModel _model;

        #endregion

        #region ctor

        public SaveFileWindow()
        {
            _model = new SaveFileViewModel(Environment.CurrentDirectory);
            InitializeComponent();
            DataContext = _model;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the status if the dialog has been accepted.
        /// </summary>
        public Boolean Accepted
        {
            get { return _model.Accepted; }
        }

        /// <summary>
        /// Gets or sets the currently selected file.
        /// </summary>
        public String SelectedFile
        {
            get { return _model.UserSelectedFile.FullName; }
            set { _model.FileName = value; }
        }

        #endregion

        #region Methods

        public void AddFilter(String name, String value)
        {
            _model.AddFilter(name, value);
        }

        public void RemoveFilter(String name)
        {
            _model.RemoveFilter(name);
        }

        #endregion

        #region Events

        void ClearSelected(Object sender, RoutedEventArgs e)
        {
            (sender as ListView).SelectedIndex = -1;

            if (sender != Current)
            {
                WaitAndFocus();
            }
        }

        async void WaitAndFocus()
        {
            //This hack seems strange but unfortunately it is the way to go
            await Task.Delay(10);
            Current.Focus();
        }

        void TextBoxKeyPressed(Object sender, KeyEventArgs e)
        {
            var tb = sender as TextBox;

            if (e.Key == Key.Enter)
            {
                _model.FileName = tb.Text;
                var path = _model.CurrentDirectory.FullName + "\\" + _model.FileName;

                if (System.IO.Directory.Exists(path))
                {
                    _model.CurrentDirectory = new FolderModel(path);
                }
                else if (_model.CanAccept)
                {
                    _model.Accept.Execute(this);
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
            _model.CanAccept = !tb.Text.Equals(String.Empty) && _model.IsValid(tb.Text);
        }

        #endregion
    }
}
