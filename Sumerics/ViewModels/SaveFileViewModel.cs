namespace Sumerics
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Windows;

    class SaveFileViewModel : FileBaseViewModel
    {
        #region Fields

        String fileName;

        #endregion

        #region ctor

        public SaveFileViewModel(String startFileOrFolder)
            : base(null)
        {
            fileName = String.Empty;
            Directories = new ObservableCollection<FolderModel>();

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

        public override string FileName
        {
            get { return fileName; }
            set
            {
                var dir = currentDirectory;

                if (Path.IsPathRooted(value))
                {
                    dir = new FolderModel(Path.GetDirectoryName(value));
                    fileName = Path.GetFileName(value);
                }
                //else if(Directory.Exists(currentDirectory.FullName + "\\" + value))
                //{
                //    dir = new FolderModel(currentDirectory.FullName + "\\" + value);
                //    fileName = string.Empty;
                //}
                else
                    fileName = value;

                CanAccept = IsValid(fileName);
                selectedFile = null;
                CurrentDirectory = dir;
            }
        }

        public FileModel UserSelectedFile
        {
            get
            {
                var path = CurrentDirectory.FullName + "\\" + fileName;
                var ext = Extension;

                if(!string.IsNullOrEmpty(ext))
                {
                    if (!Path.GetExtension(path).ToLower().Equals(ext.ToLower()))
                        path += Extension;
                }

                return new FileModel(path);
            }
        }

        public override FileModel SelectedFile
        {
            get { return selectedFile ?? UserSelectedFile; }
            set
            {
                if (value == null)
                    return;

                if (value.IsDirectory)
                {
                    CurrentDirectory = new FolderModel(value.FullName);
                    return;
                }

                fileName = Path.GetFileName(value.FullName);
                selectedFile = value;
                currentDirectory = selectedFile.Folder;
                CanAccept = true;
                AllChanged();
            }
        }

        #endregion

        #region Methods

        public bool IsValid(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return false;

            var inv = Path.GetInvalidFileNameChars();
            return !fileName.Intersect(inv).Any();
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
                    return;
            }

            base.OnAccept(window);
        }

        #endregion
    }
}
