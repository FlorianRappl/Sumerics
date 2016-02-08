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
        #region Members

        SaveFileViewModel model;

        #endregion

        #region ctor

        public SaveFileWindow(IContainer container)
        {
            model = new SaveFileViewModel(Environment.CurrentDirectory, container);
            InitializeComponent();
            DataContext = model;
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
                return model.Accepted;
            }
        }

        /// <summary>
        /// Gets or sets the currently selected file.
        /// </summary>
        public string SelectedFile
        {
            get
            {
                return model.UserSelectedFile.FullName;
            }
            set
            {
                model.FileName = value;
            }
        }

        #endregion

        #region Methods

        public void AddFilter(string name, string value)
        {
            model.AddFilter(name, value);
        }

        public void RemoveFilter(string name)
        {
            model.RemoveFilter(name);
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
                model.FileName = tb.Text;
                var path = model.CurrentDirectory.FullName + "\\" + model.FileName;

                if (System.IO.Directory.Exists(path))
                    model.CurrentDirectory = new FolderModel(path);
                else if (model.CanAccept)
                    model.Accept.Execute(this);
            }
            else if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        void TextBoxChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            model.CanAccept = !tb.Text.Equals(string.Empty) && model.IsValid(tb.Text);
        }

        #endregion
    }
}
