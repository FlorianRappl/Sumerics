namespace Sumerics.Dialogs
{
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
            dialog.Title = "Open workspace ...";
            dialog.AddFilter("All files (*.*)", "*.*");
            dialog.AddFilter("Sumerics workspace (*.sws)", "*.sws");
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
