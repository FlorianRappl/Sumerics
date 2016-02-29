namespace Sumerics.Dialogs
{
    using Sumerics.Resources;
    using Sumerics.Views;
    using System;

    [DialogType(Dialog.LoadWorkspace)]
    sealed class LoadWorkspaceHandler : IDialogHandler
    {
        readonly IComponents _container;
        readonly IKernel _kernel;

        public LoadWorkspaceHandler(IComponents container, IKernel kernel)
        {
            _container = container;
            _kernel = kernel;
        }

        public void Open(params Object[] parameters)
        {
            var dialog = _container.Obtain<OpenFileWindow>();
            dialog.Title = Messages.OpenWorkspace;
            dialog.AddFilter(Messages.AllFiles + " (*.*)", "*.*");
            dialog.AddFilter(Messages.SumericsWorkspace + " (*.sws)", "*.sws");
            dialog.ShowDialog();

            if (dialog.Accepted)
            {
                _kernel.LoadWorkspaceAsync(dialog.SelectedFile);
            }
        }

        public void Close()
        {
        }
    }
}
