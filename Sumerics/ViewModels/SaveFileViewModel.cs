namespace Sumerics.ViewModels
{
    using Sumerics.Models;
    using Sumerics.Views;
    using System;
    using System.IO;
    using System.Linq;
    using System.Windows;

    public class SaveFileViewModel : FileBaseViewModel
    {
        #region Fields

        String _fileName;

        #endregion

        #region ctor

        public SaveFileViewModel(String startFileOrFolder)
        {
            _fileName = String.Empty;

            if (Directory.Exists(startFileOrFolder))
            {
                CurrentDirectory = new FolderModel(startFileOrFolder);
            }
            else
            {
                SelectedFile = new FileModel(startFileOrFolder);
            }
        }

        #endregion

        #region Properties

        public override String FileName
        {
            get { return _fileName; }
            set
            {
                var dir = _currentDirectory;

                if (Path.IsPathRooted(value))
                {
                    dir = new FolderModel(Path.GetDirectoryName(value));
                    _fileName = Path.GetFileName(value);
                }
                //else if(Directory.Exists(currentDirectory.FullName + "\\" + value))
                //{
                //    dir = new FolderModel(currentDirectory.FullName + "\\" + value);
                //    fileName = string.Empty;
                //}
                else
                {
                    _fileName = value;
                }

                CanAccept = IsValid(_fileName);
                _selectedFile = null;
                CurrentDirectory = dir;
            }
        }

        public FileModel UserSelectedFile
        {
            get
            {
                var path = CurrentDirectory.FullName + "\\" + _fileName;
                var ext = Extension;

                if (!String.IsNullOrEmpty(ext))
                {
                    if (!Path.GetExtension(path).Equals(ext, StringComparison.InvariantCultureIgnoreCase))
                    {
                        path += Extension;
                    }
                }

                return new FileModel(path);
            }
        }

        public override FileModel SelectedFile
        {
            get { return _selectedFile ?? UserSelectedFile; }
            set
            {
                if (value != null)
                {
                    if (value.IsDirectory)
                    {
                        CurrentDirectory = new FolderModel(value.FullName);
                        return;
                    }

                    _fileName = Path.GetFileName(value.FullName);
                    _selectedFile = value;
                    _currentDirectory = _selectedFile.Folder;
                    CanAccept = true;
                    AllChanged();
                }
            }
        }

        #endregion

        #region Methods

        public Boolean IsValid(String fileName)
        {
            if (!String.IsNullOrEmpty(fileName))
            {
                var inv = Path.GetInvalidFileNameChars();
                return !fileName.Intersect(inv).Any();
            }

            return false;
        }

        protected override void OnAccept(Window window)
        {
            var path = UserSelectedFile.FullName;

            if (File.Exists(path))
            {
                var fn = Path.GetFileName(path);

                var result = DecisionDialog.Show("Do you want to overwrite the file " + fn + "?", new[] {
                    "Sure, go ahead and overwrite it.", "I am not sure, please cancel."
                });

                if (result == 1)
                {
                    return;
                }
            }

            base.OnAccept(window);
        }

        #endregion
    }
}
