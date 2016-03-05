namespace Sumerics.Dialogs
{
    using Sumerics.ViewModels;
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
