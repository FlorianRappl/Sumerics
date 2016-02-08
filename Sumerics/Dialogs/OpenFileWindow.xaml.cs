namespace Sumerics
{
    using MahApps.Metro.Controls;
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
        #region Members

        OpenFileViewModel model;

        #endregion

        #region ctor

        public OpenFileWindow(IContainer container)
        {
            model = new OpenFileViewModel(Environment.CurrentDirectory, container);
            InitializeComponent();
            DataContext = model;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value if the dialog was accepted, i.e.
        /// some file got picked.
        /// </summary>
        public bool Accepted
        {
            get
            {
                return model.Accepted;
            }
        }

        /// <summary>
        /// Gets or sets the path to the currently selected file.
        /// </summary>
        public string SelectedFile
        {
            get
            {
                return model.SelectedFile.FullName;
            }
            set
            {
                model.SelectedFile = new FileModel(value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a filer to the dialog.
        /// </summary>
        /// <param name="name">The description.</param>
        /// <param name="value">The value of the filter.</param>
        public void AddFilter(string name, string value)
        {
            model.AddFilter(name, value);
        }

        /// <summary>
        /// Removes a filter from the dialog.
        /// </summary>
        /// <param name="name">The description.</param>
        public void RemoveFilter(string name)
        {
            model.RemoveFilter(name);
        }

        #endregion

        #region Events

        void ClearSelected(object sender, RoutedEventArgs e)
        {
            (sender as ListView).SelectedIndex = -1;

            if(sender != Current)
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
            model.CanAccept = System.IO.File.Exists(model.CurrentDirectory.FullName + "\\" + tb.Text);
        }

        #endregion
    }
}
