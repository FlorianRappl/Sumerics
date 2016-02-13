namespace Sumerics.Dialogs
{
    using Sumerics.Views;
    using System;

    [DialogType(Dialog.Directory)]
    sealed class DirectoryDialogHandler : IDialogHandler
    {
        readonly IComponents _container;

        public DirectoryDialogHandler(IComponents container)
        {
            _container = container;
        }

        public void Open(params Object[] parameters)
        {
            var dialog = _container.Obtain<FolderBrowseWindow>();
            dialog.ShowDialog();

            if (dialog.Accepted)
            {
                Environment.CurrentDirectory = dialog.SelectedDirectory;
            }
        }

        public void Close()
        {
        }
    }
}
