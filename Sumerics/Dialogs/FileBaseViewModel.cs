namespace Sumerics
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Windows.Input;

    abstract class FileBaseViewModel : DialogBaseViewModel
    {
        #region Fields

        protected FolderModel currentDirectory;
        protected FileModel selectedFile;

        Dictionary<String, String> filters;
        String selectedFilter;

        #endregion

        #region ctor

        public FileBaseViewModel(IContainer container)
            : base(container)
        {
            Files = new ObservableCollection<FileModel>();
            filters = new Dictionary<String, String>();
            Filters = new ObservableCollection<String>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the picked extension. If the extension is arbitrary (.*) then
        /// string.Empty is returned.
        /// </summary>
        public string Extension
        {
            get
            {
                var filter = Path.GetExtension(filters[selectedFilter]);

                if (filter == ".*")
                    return string.Empty;

                return filter;
            }
        }

        /// <summary>
        /// Gets or sets the current directory.
        /// </summary>
        public FolderModel CurrentDirectory
        {
            get { return currentDirectory; }
            set
            {
                if (value == null)
                    return;

                SetPathToWatch(value.FullName);
                currentDirectory = value;
                UpdateDirectoryHistory();
                SelectedFile = null;
                PopulateFiles();
                AllChanged();
            }
        }

        /// <summary>
        /// Sets the current directory to the selected place.
        /// </summary>
        public override FolderModel SelectedPlace
        {
            get { return null; }
            set { CurrentDirectory = value; }
        }

        public abstract FileModel SelectedFile { get; set; }

        public abstract string FileName { get; set; }

        /// <summary>
        /// Changes the current directory.
        /// </summary>
        public ICommand ChangeDirectory
        {
            get
            {
                return new RelayCommand(x =>
                {
                    var y = x as FolderModel;

                    if (y != null)
                        CurrentDirectory = y;
                });
            }
        }

        /// <summary>
        /// Gets or sets the list with directories of the current directory.
        /// </summary>
        public ObservableCollection<FolderModel> Directories
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the list with files of the current directory.
        /// </summary>
        public ObservableCollection<FileModel> Files
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the list with file filters.
        /// </summary>
        public ObservableCollection<string> Filters
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the selected filter.
        /// </summary>
        public string SelectedFilter
        {
            get { return selectedFilter; }
            set
            {
                selectedFilter = value;
                PopulateFiles();
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Watcher Method

        protected override void RaiseDirectoryChanged()
        {
            PopulateFiles();
            SelectedFile = selectedFile;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a file (extension) filter to the dialog.
        /// </summary>
        /// <param name="name">The name, i.e. the description.</param>
        /// <param name="value">The value, i.e. like .jpg or .png or similar.</param>
        public void AddFilter(string name, string value)
        {
            Filters.Add(name);
            filters.Add(name, value);

            if (filters.Count == 1)
                SelectedFilter = name;
        }

        /// <summary>
        /// Removes a file (extension) filter from the dialog.
        /// </summary>
        /// <param name="name">The name, i.e. the description, of the filter to remove.</param>
        public void RemoveFilter(string name)
        {
            Filters.Remove(name);
            filters.Remove(name);

            if (selectedFilter == name)
                SelectedFilter = Filters.Count > 0 ? Filters[0] : string.Empty;
        }

        /// <summary>
        /// Populates the list of files of the current directory.
        /// </summary>
        protected void PopulateFiles()
        {
            Files.Clear();

            var folders = new DirectoryInfo[0];

            try
            {
                folders = CurrentDirectory.Info.GetDirectories();
            }
            catch { }

            foreach (var folder in folders)
                Files.Add(new FileModel(folder));

            var files = new FileInfo[0];

            try
            {
                if (string.IsNullOrEmpty(SelectedFilter))
                    files = CurrentDirectory.Info.GetFiles();
                else
                    files = CurrentDirectory.Info.GetFiles(filters[selectedFilter]);
            }
            catch { }

            foreach (var file in files)
                Files.Add(new FileModel(file));
        }

        protected virtual void UpdateDirectoryHistory()
        {

            Directories.Clear();
            var current = currentDirectory;

            do
            {
                Directories.Insert(0, current);
                current = current.Parent;
            }
            while (Directories.Count < 3 && current != null);
        }

        /// <summary>
        /// Calls the changed event of all important properties.
        /// </summary>
        protected virtual void AllChanged()
        {
            RaisePropertyChanged("CurrentDirectory");
            RaisePropertyChanged("PreviousDirectory");
            RaisePropertyChanged("PreviousPreviousDirectory");
            RaisePropertyChanged("SelectedFile");
            RaisePropertyChanged("FileName");
        }

        #endregion
    }
}
