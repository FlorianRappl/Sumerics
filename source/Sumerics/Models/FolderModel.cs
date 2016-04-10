namespace Sumerics.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Represents the encapsulated information of a folder.
    /// </summary>
    public sealed class FolderModel
    {
        #region Fields

        readonly BitmapImage _icon;

        #endregion

        #region ctor

        public FolderModel(DriveInfo drive)
        {
            Info = new DirectoryInfo(drive.Name);
            IsDrive = true;
            _icon = IconFactory.HomeIcon;
        }

        public FolderModel(DirectoryInfo directory)
        {
            Info = directory;
            IsDrive = directory.Parent == null;
            _icon = IsDrive ? IconFactory.HomeIcon : IconFactory.FolderIcon;
        }

        public FolderModel(String directory) : 
            this(new DirectoryInfo(directory))
        {
        }

        public FolderModel(Environment.SpecialFolder directory) : 
            this(Environment.GetFolderPath(directory))
        {
            _icon = IconFactory.HeartIcon;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the folder.
        /// </summary>
        public String Name 
        { 
            get { return Info.Name; } 
        }

        /// <summary>
        /// Gets the path of the folder.
        /// </summary>
        public String FullName 
        { 
            get { return Info.FullName; } 
        }

        /// <summary>
        /// Gets the information if the folder is a drive.
        /// </summary>
        public Boolean IsDrive 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// Gets the underlying directory info object of the folder.
        /// </summary>
        public DirectoryInfo Info 
        { 
            get;
            private set; 
        }

        /// <summary>
        /// Gets the parent directory of the folder.
        /// </summary>
        public FolderModel Parent 
        { 
            get { return IsDrive ? null : new FolderModel(Info.Parent); } 
        }

        /// <summary>
        /// Gets the associated icon of the folder.
        /// </summary>
        public BitmapImage Icon
        { 
            get { return _icon; } 
        }

        #endregion

        #region Directories

        public IEnumerable<FolderModel> TopDirectories
        {
            get
            {
                if (!IsDrive)
                {
                    var parent = new FolderModel(Info.Parent);
                    var directories = parent.Directories;

                    foreach (var directory in directories)
                    {
                        yield return directory;
                    }
                }
            }
        }

        public IEnumerable<FolderModel> SubDirectories
        {
            get
            {
                var directories = new DirectoryInfo[0];

                try
                {
                    directories = Info.GetDirectories();
                }
                catch
                { }

                foreach (var directory in directories)
                {
                    yield return new FolderModel(directory);
                }
            }
        }

        public IEnumerable<FolderModel> Directories
        {
            get
            {
                if (IsDrive)
                {
                    var drives = new DriveInfo[0];

                    try
                    {
                        drives = DriveInfo.GetDrives();
                    }
                    catch
                    { }

                    foreach (var drive in drives)
                    {
                        yield return new FolderModel(drive);
                    }
                }
                else
                {
                    var directories = new DirectoryInfo[0];

                    try
                    {
                        directories = Info.Parent.GetDirectories();
                    }
                    catch
                    { }

                    foreach (var directory in directories)
                    {
                        yield return new FolderModel(directory);
                    }
                }
            }
        }

        #endregion

        #region Methods

        public override Boolean Equals(Object obj)
        {
            if (obj is FolderModel)
            {
                var target = obj as FolderModel;
                return FullName.Equals(target.FullName);
            }

            return false;
        }

        public override Int32 GetHashCode()
        {
            return FullName.GetHashCode();
        }

        #endregion

        #region Helpers

        public static IEnumerable<FolderModel> GetDrives()
        {
            var drives = DriveInfo.GetDrives();

            foreach (var drive in drives)
            {
                yield return new FolderModel(drive);
            }
        }

        #endregion
    }
}
