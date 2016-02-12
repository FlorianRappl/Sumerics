namespace Sumerics.ViewModels
{
    using Sumerics.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;

    sealed class FolderBrowseViewModel : DialogBaseViewModel
    {
        #region Fields

        FolderModel _selectedDirectory;
        FolderModel _selectedTopDirectory;

        #endregion

        #region ctor

        public FolderBrowseViewModel(String startFolder)
        {
            CanAccept = true;
            CurrentDirectories = new ObservableCollection<FolderModel>();
            SubDirectories = new ObservableCollection<FolderModel>();
            TopDirectories = new ObservableCollection<FolderModel>();

            if (Directory.Exists(startFolder))
            {
                SelectedDirectory = new FolderModel(startFolder);
            }
            else
            {
                SelectedDirectory = new FolderModel(Environment.CurrentDirectory);
            }
        }

        #endregion

        #region Properties

        public override FolderModel SelectedPlace
        {
            get { return null; }
            set { SelectedDirectory = value; }
        }

        public FolderModel SelectedSubDirectory
        {
            get { return null; }
            set { SelectedDirectory = value; }
        }

        public FolderModel SelectedTopDirectory
        {
            get { return _selectedTopDirectory; }
            set { SelectedDirectory = value; }
        }

        public FolderModel SelectedDirectory
        {
            get
            { 
                return _selectedDirectory; 
            }
            set
            {
                if (value != null)
                {
                    SetPathToWatch(value.FullName);
                    ChangeDirectory(value);
                    _selectedDirectory = value;
                    _selectedTopDirectory = value.Parent;

                    RaisePropertyChanged("SelectedPlace");
                    RaisePropertyChanged("SelectedDirectory");
                    RaisePropertyChanged("SelectedTopDirectory");
                    RaisePropertyChanged("SelectedSubDirectory");
                }
            }
        }

        public ObservableCollection<FolderModel> CurrentDirectories
        {
            get;
            set;
        }

        public ObservableCollection<FolderModel> SubDirectories
        {
            get;
            set;
        }

        public ObservableCollection<FolderModel> TopDirectories
        {
            get;
            set;
        }

        #endregion

        #region Methods

        protected override void RaiseDirectoryChanged()
        {
            LoadDirectories(TopDirectories, _selectedDirectory.TopDirectories);
        }

        void ChangeDirectory(FolderModel directory)
        {
            LoadDirectories(TopDirectories, directory.TopDirectories);
            LoadDirectories(CurrentDirectories, directory.Directories);
            LoadDirectories(SubDirectories, directory.SubDirectories);
        }

        void LoadDirectories(ObservableCollection<FolderModel> list, IEnumerable<FolderModel> directories)
        {
            list.Clear();

            foreach (var directory in directories)
            {
                list.Add(directory);
            }
        }

        #endregion
    }
}
