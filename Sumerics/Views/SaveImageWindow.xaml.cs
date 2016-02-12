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
    public partial class SaveImageWindow : MetroWindow
    {
        #region Fields

        readonly SaveImageViewModel _model;

        #endregion

        #region ctor

        public SaveImageWindow()
        {
            _model = new SaveImageViewModel(Environment.CurrentDirectory);
            InitializeComponent();
            DataContext = _model;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the status if the dialog has been accepted.
        /// </summary>
        public bool Accepted
        {
            get
            {
                return _model.Accepted;
            }
        }

        /// <summary>
        /// Gets or sets the currently selected file.
        /// </summary>
        public string SelectedFile
        {
            get
            {
                return _model.UserSelectedFile.FullName;
            }
            set
            {
                _model.FileName = value;
            }
        }

        /// <summary>
        /// Gets or sets currently selected the image width.
        /// </summary>
        public int ImageWidth
        {
            get
            {
                return _model.ImageWidth;
            }
            set
            {
                _model.ImageWidth = value;
            }
        }

        /// <summary>
        /// Gets or sets the currently selected image height.
        /// </summary>
        public int ImageHeight
        {
            get
            {
                return _model.ImageHeight;
            }
            set
            {
                _model.ImageHeight = value;
            }
        }

        #endregion

        #region Events

        void ClearSelected(object sender, RoutedEventArgs e)
        {
            (sender as ListView).SelectedIndex = -1;
            
            if (sender != Current)
                WaitAndFocus();
        }

        async void WaitAndFocus()
        {
            //This hack seems strange but unfortunately it is the way to go
            await Task.Delay(10);
            Current.Focus();
        }

        void TextBoxKeyPressed(object sender, KeyEventArgs e)
        {
            var tb = sender as TextBox;

            if (e.Key == Key.Enter)
            {
                _model.FileName = tb.Text;
                var path = _model.CurrentDirectory.FullName + "\\" + _model.FileName;

                if (System.IO.Directory.Exists(path))
                    _model.CurrentDirectory = new FolderModel(path);
                else if (_model.CanAccept)
                    _model.Accept.Execute(this);
            }
            else if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        void TextBoxChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            _model.CanAccept = !tb.Text.Equals(string.Empty) && _model.IsValid(tb.Text);
        }

        #endregion
    }
}