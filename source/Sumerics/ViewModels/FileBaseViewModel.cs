namespace Sumerics.ViewModels
{
    using Sumerics.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Windows.Input;

    public abstract class FileBaseViewModel : DialogBaseViewModel
    {
        #region Fields

        protected FolderModel _currentDirectory;
        protected FileModel _selectedFile;

        readonly ObservableCollection<FolderModel> _directories;
        readonly ObservableCollection<FileModel> _files;
        readonly ObservableCollection<String> _availableFilters;
        readonly Dictionary<String, String> _filters;
        readonly ICommand _change;

        String _selectedFilter;

        #endregion

        #region ctor

        public FileBaseViewModel()
        {
            _directories = new ObservableCollection<FolderModel>();
            _files = new ObservableCollection<FileModel>();
            _filters = new Dictionary<String, String>();
            _availableFilters = new ObservableCollection<String>();
            _change = new RelayCommand(x =>
            {
                var y = x as FolderModel;

                if (y != null)
                {
                    CurrentDirectory = y;
                }
            });
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the picked extension. If the extension is arbitrary (.*) then
        /// string.Empty is returned.
        /// </summary>
        public String Extension
        {
            get
            {
                var filter = Path.GetExtension(_filters[_selectedFilter]);

                if (filter == ".*")
                {
                    return String.Empty;
                }

                return filter;
            }
        }

        /// <summary>
        /// Gets or sets the current directory.
        /// </summary>
        public FolderModel CurrentDirectory
        {
            get { return _currentDirectory; }
            set
            {
                if (value != null)
                {
                    SetPathToWatch(value.FullName);
                    _currentDirectory = value;
                    UpdateDirectoryHistory();
                    SelectedFile = null;
                    PopulateFiles();
                    AllChanged();
                }
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

        public abstract FileModel SelectedFile
        { 
            get; 
            set; 
        }

        public abstract String FileName 
        { 
            get;
            set; 
        }

        /// <summary>
        /// Changes the current directory.
        /// </summary>
        public ICommand ChangeDirectory
        {
            get { return _change; }
        }

        /// <summary>
        /// Gets or sets the list with directories of the current directory.
        /// </summary>
        public ObservableCollection<FolderModel> Directories
        {
            get { return _directories; }
        }

        /// <summary>
        /// Gets or sets the list with files of the current directory.
        /// </summary>
        public ObservableCollection<FileModel> Files
        {
            get { return _files; }
        }

        /// <summary>
        /// Gets or sets the list with file filters.
        /// </summary>
        public ObservableCollection<String> Filters
        {
            get { return _availableFilters; }
        }

        /// <summary>
        /// Gets or sets the selected filter.
        /// </summary>
        public String SelectedFilter
        {
            get { return _selectedFilter; }
            set
            {
                _selectedFilter = value;
                PopulateFiles();
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Watcher Method

        protected override void RaiseDirectoryChanged()
        {
            PopulateFiles();
            SelectedFile = _selectedFile;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a file (extension) filter to the dialog.
        /// </summary>
        /// <param name="name">The name, i.e. the description.</param>
        /// <param name="value">The value, i.e. like .jpg or .png or similar.</param>
        public void AddFilter(String name, String value)
        {
            Filters.Add(name);
            _filters.Add(name, value);

            if (_filters.Count == 1)
            {
                SelectedFilter = name;
            }
        }

        /// <summary>
        /// Removes a file (extension) filter from the dialog.
        /// </summary>
        /// <param name="name">The name, i.e. the description, of the filter to remove.</param>
        public void RemoveFilter(String name)
        {
            Filters.Remove(name);
            _filters.Remove(name);

            if (_selectedFilter == name)
            {
                SelectedFilter = Filters.Count > 0 ? Filters[0] : String.Empty;
            }
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
            {
                Files.Add(new FileModel(folder));
            }

            var files = new FileInfo[0];

            try
            {
                if (String.IsNullOrEmpty(SelectedFilter))
                {
                    files = CurrentDirectory.Info.GetFiles();
                }
                else
                {
                    files = CurrentDirectory.Info.GetFiles(_filters[_selectedFilter]);
                }
            }
            catch { }

            foreach (var file in files)
            {
                Files.Add(new FileModel(file));
            }
        }

        protected virtual void UpdateDirectoryHistory()
        {
            Directories.Clear();
            var current = _currentDirectory;

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
