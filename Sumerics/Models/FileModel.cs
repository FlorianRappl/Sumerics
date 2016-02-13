namespace Sumerics.Models
{
    using System;
    using System.IO;

    /// <summary>
    /// The file model.
    /// </summary>
    public sealed class FileModel
    {
        #region ctor

        /// <summary>
        /// Creates a new file model.
        /// </summary>
        /// <param name="file">The basic file information.</param>
        public FileModel(FileInfo file)
        {
            Info = file;
        }

        /// <summary>
        /// Creates a new file model.
        /// </summary>
        /// <param name="startFile">The path to the file.</param>
        public FileModel(String startFile) : 
            this(new FileInfo(startFile))
        {
        }

        /// <summary>
        /// Creates a new file model.
        /// </summary>
        /// <param name="directory">The basic directory information.</param>
        public FileModel(DirectoryInfo directory) : 
            this(new FileInfo(directory.FullName))
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the file or directory
        /// </summary>
        public String Name 
        { 
            get { return Info.Name; } 
        }

        /// <summary>
        /// Gets the path of the file or directory.
        /// </summary>
        public String FullName 
        { 
            get { return Info.FullName; } 
        }

        /// <summary>
        /// Gets the information about the file or directory.
        /// </summary>
        public FileInfo Info 
        { 
            get;
            protected set; 
        }

        /// <summary>
        /// Gets the associated icon of the file or directory.
        /// </summary>
        public Object Icon 
        { 
            get { return IsDirectory ? Icons.FolderIcon : Icons.FileIcon; }
        }

        /// <summary>
        /// Gets the status - is the file a directory?
        /// </summary>
        public Boolean IsDirectory 
        { 
            get {  return (Info.Attributes & FileAttributes.Directory) == FileAttributes.Directory && Directory.Exists(Info.FullName); } 
        }

        /// <summary>
        /// Gets the status - is the file valid, i.e. does it exist?
        /// </summary>
        public Boolean IsValid 
        { 
            get { return Info.Exists; } 
        }

        /// <summary>
        /// Gets the associated folder of the file.
        /// </summary>
        public FolderModel Folder
        { 
            get { return new FolderModel(Info.Directory); } 
        }

        #endregion

        #region Equality
        
        public override Boolean Equals(Object obj)
        {
            if (obj is FileModel)
            {
                var target = (FileModel)obj;
                return target.FullName.Equals(FullName, StringComparison.CurrentCultureIgnoreCase);
            }

            return false;
        }

        public override Int32 GetHashCode()
        {
            return FullName.GetHashCode();
        }

        #endregion
    }
}
