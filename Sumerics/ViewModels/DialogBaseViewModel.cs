namespace Sumerics.ViewModels
{
    using Sumerics.Models;
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Windows;
    using System.Windows.Input;

    public abstract class DialogBaseViewModel : BaseViewModel
    {
        #region Fields

        readonly ICommand _accept;
        readonly FileSystemWatcher _watcher;
        readonly ObservableCollection<FolderModel> _places;
        Boolean _canAccept;

        #endregion

        #region ctor

        public DialogBaseViewModel()
        {
            _places = new ObservableCollection<FolderModel>();
            PopulatePlaces();

            _watcher = new FileSystemWatcher();
            _watcher.IncludeSubdirectories = false;
            _watcher.Deleted += FileWatcherReport;
            _watcher.Created += FileWatcherReport;
            _watcher.Renamed += FileWatcherReport;

            _accept = new RelayCommand(x => OnAccept(x as Window));
        }

        #endregion

        #region Properties

        public abstract FolderModel SelectedPlace
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the list of available special places (fast access).
        /// </summary>
        public ObservableCollection<FolderModel> Places
        {
            get { return _places; }
        }

        /// <summary>
        /// Gets or sets if the dialog can be accepted.
        /// </summary>
        public Boolean CanAccept
        {
            get { return _canAccept; }
            set
            {
                _canAccept = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Accepts the command, i.e. closes it and sets the Accepted to true.
        /// </summary>
        public ICommand Accept
        {
            get { return _accept; }
        }

        /// <summary>
        /// Gets or sets if the dialog has been accepted.
        /// </summary>
        public Boolean Accepted
        {
            get;
            set;
        }

        #endregion

        #region File System Watcher

        protected virtual void RaiseDirectoryChanged()
        {
        }

        protected void SetPathToWatch(String path)
        {
            _watcher.EnableRaisingEvents = false;
            _watcher.Path = path;
            _watcher.EnableRaisingEvents = true;
        }

        void FileWatcherReport(Object sender, FileSystemEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() => RaiseDirectoryChanged());
        }

        #endregion

        #region Methods

        protected virtual void OnAccept(Window window)
        {
            Accepted = true;

            if (window != null)
            {
                window.Close();
            }
        }

        /// <summary>
        /// Populates the list with places.
        /// </summary>
        protected virtual void PopulatePlaces()
        {
            Places.Add(new FolderModel(Environment.SpecialFolder.Desktop));
            Places.Add(new FolderModel(Environment.SpecialFolder.MyDocuments));
            var drives = FolderModel.GetDrives();

            foreach (var drive in drives)
            {
                Places.Add(drive);
            }
        }

        #endregion
    }
}
