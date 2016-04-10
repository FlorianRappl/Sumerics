namespace Sumerics.ViewModels
{
    using Sumerics.Models;
    using Sumerics.Resources;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;

    public sealed class FolderBrowseViewModel : DialogBaseViewModel
    {
        #region Fields

        readonly ObservableCollection<FolderModel> _currentDirectories;
        readonly ObservableCollection<FolderModel> _subDirectories;
        readonly ObservableCollection<FolderModel> _topDirectories;

        FolderModel _selectedDirectory;
        FolderModel _selectedTopDirectory;

        #endregion

        #region ctor

        public FolderBrowseViewModel()
            : this(Environment.CurrentDirectory)
        {
        }

        public FolderBrowseViewModel(String startFolder)
        {
            Title = Messages.SelectFolder;
            CanAccept = true;
            _currentDirectories = new ObservableCollection<FolderModel>();
            _subDirectories = new ObservableCollection<FolderModel>();
            _topDirectories = new ObservableCollection<FolderModel>();

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
            get { return _currentDirectories; }
        }

        public ObservableCollection<FolderModel> SubDirectories
        {
            get { return _subDirectories; }
        }

        public ObservableCollection<FolderModel> TopDirectories
        {
            get { return _topDirectories; }
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
