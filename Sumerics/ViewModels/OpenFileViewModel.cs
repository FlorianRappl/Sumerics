namespace Sumerics
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;

    sealed class OpenFileViewModel : FileBaseViewModel
    {
        #region ctor

        public OpenFileViewModel(String startFileOrFolder, IContainer container)
            : base(container)
        {
            Directories = new ObservableCollection<FolderModel>();

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

        public override string FileName
        {
            get 
            {
                if (SelectedFile == null) 
                    return string.Empty; 

                return SelectedFile.Name; 
            }
            set
            {
                if (Path.IsPathRooted(value))
                    SelectedFile = new FileModel(value);
                else
                    SelectedFile = new FileModel(CurrentDirectory.FullName + "\\" + value);
            }
        }

        public override FileModel SelectedFile
        {
            get { return selectedFile; }
            set
            {
                CanAccept = false;

                if (value == null)
                    return;

                if (value.IsDirectory)
                {
                    CurrentDirectory = new FolderModel(value.FullName);
                    return;
                }

                CanAccept = value.IsValid;
                selectedFile = value;
                currentDirectory = selectedFile.Folder;
                AllChanged();
            }
        }

        #endregion
    }
}
