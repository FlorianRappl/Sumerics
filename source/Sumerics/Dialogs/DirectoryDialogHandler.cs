namespace Sumerics.Dialogs
{
    using Sumerics.ViewModels;
    using System;

    [DialogType(Dialog.Directory)]
    sealed class DirectoryDialogHandler : IDialogHandler
    {
        public void Open(params Object[] parameters)
        {
            var context = new FolderBrowseViewModel();
            context.ShowDialog();

            if (context.Accepted)
            {
                Environment.CurrentDirectory = context.SelectedDirectory.FullName;
            }
        }

        public void Close()
        {
        }
    }
}
