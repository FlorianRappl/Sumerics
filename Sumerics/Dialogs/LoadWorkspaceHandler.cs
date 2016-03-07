namespace Sumerics.Dialogs
{
    using Sumerics.Resources;
    using Sumerics.ViewModels;
    using System;

    [DialogType(Dialog.LoadWorkspace)]
    sealed class LoadWorkspaceHandler : IDialogHandler
    {
        readonly IKernel _kernel;

        public LoadWorkspaceHandler(IKernel kernel)
        {
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
