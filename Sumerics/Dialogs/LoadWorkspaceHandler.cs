namespace Sumerics.Dialogs
{
    using Sumerics.Resources;
    using Sumerics.ViewModels;
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
            var context = new OpenFileViewModel();
            context.Title = Messages.OpenWorkspace;
            context.AddFilter(Messages.AllFiles + " (*.*)", "*.*");
            context.AddFilter(Messages.SumericsWorkspace + " (*.sws)", "*.sws");
            context.ShowDialog();

            if (context.Accepted)
            {
                _kernel.LoadWorkspaceAsync(context.SelectedFile.FullName);
            }
        }

        public void Close()
        {
        }
    }
}
