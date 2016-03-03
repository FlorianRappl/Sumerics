namespace Sumerics.ViewModels
{
    using Sumerics.Models;
    using System;
    using System.IO;

    public sealed class OpenFileViewModel : FileBaseViewModel
    {
        #region ctor

        public OpenFileViewModel()
            : this(Environment.CurrentDirectory)
        {
        }

        public OpenFileViewModel(String startFileOrFolder)
        {
            if (File.Exists(startFileOrFolder))
            {
                SelectedFile = new FileModel(startFileOrFolder);
            }
            else if (Directory.Exists(startFileOrFolder))
            {
                CurrentDirectory = new FolderModel(startFileOrFolder);
            }
            else
            {
                CurrentDirectory = new FolderModel(Environment.CurrentDirectory);
            }
        }

        #endregion

        #region Properties

        public override String FileName
        {
            get 
            {
                if (SelectedFile == null)
                {
                    return String.Empty;
                }

                return SelectedFile.Name; 
            }
            set
            {
                if (Path.IsPathRooted(value))
                {
                    SelectedFile = new FileModel(value);
                }
                else
                {
                    var path = Path.Combine(CurrentDirectory.FullName, value);
                    SelectedFile = new FileModel(path);
                }
            }
        }

        public override FileModel SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                CanAccept = false;

                if (value != null)
                {
                    if (value.IsDirectory)
                    {
                        CurrentDirectory = new FolderModel(value.FullName);
                        return;
                    }

                    CanAccept = value.IsValid;
                    _selectedFile = value;
                    _currentDirectory = _selectedFile.Folder;
                    AllChanged();
                }
            }
        }

        #endregion
    }
}
