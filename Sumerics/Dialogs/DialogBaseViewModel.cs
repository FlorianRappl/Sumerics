using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Sumerics
{
    abstract class DialogBaseViewModel : BaseViewModel
    {
        #region Members

        bool canAccept;
        FileSystemWatcher watcher;

        #endregion

        #region ctor

        public DialogBaseViewModel()
        {
            Places = new ObservableCollection<FolderModel>();
            PopulatePlaces();

            watcher = new FileSystemWatcher();
            watcher.IncludeSubdirectories = false;
            watcher.Deleted += FileWatcherReport;
            watcher.Created += FileWatcherReport;
            watcher.Renamed += FileWatcherReport;
        }

        #endregion

        #region Properties

        public abstract FolderModel SelectedPlace { get; set; }

        /// <summary>
        /// Gets or sets the list of available special places (fast access).
        /// </summary>
        public ObservableCollection<FolderModel> Places
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets if the dialog can be accepted.
        /// </summary>
        public bool CanAccept
        {
            get { return canAccept; }
            set
            {
                canAccept = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Accepts the command, i.e. closes it and sets the Accepted to true.
        /// </summary>
        public ICommand Accept
        {
            get
            {
                return new RelayCommand(x =>
                {
                    OnAccept(x as Window);
                });
            }
        }

        /// <summary>
        /// Gets or sets if the dialog has been accepted.
        /// </summary>
        public bool Accepted
        {
            get;
            set;
        }

        #endregion

        #region File System Watcher

        protected virtual void RaiseDirectoryChanged()
        {
        }

        protected void SetPathToWatch(string path)
        {
            watcher.EnableRaisingEvents = false;
            watcher.Path = path;
            watcher.EnableRaisingEvents = true;
        }

        void FileWatcherReport(object sender, FileSystemEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() => RaiseDirectoryChanged());
        }

        #endregion

        #region Methods

        protected virtual void OnAccept(Window window)
        {
            Accepted = true;

            if (window != null)
                window.Close();
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
                Places.Add(drive);
        }

        #endregion
    }
}
