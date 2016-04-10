namespace Sumerics.Dialogs
{
    using Sumerics.Resources;
    using Sumerics.ViewModels;
    using System;

    [DialogType(Dialog.SaveWorkspace)]
    sealed class SaveWorkspaceDialogHandler : IDialogHandler
    {
        readonly IKernel _kernel;

        public SaveWorkspaceDialogHandler(IKernel kernel)
        {
            _kernel = kernel;
        }

        public void Open(params Object[] parameters)
        {
            var context = new SaveFileViewModel();
            context.Title = Messages.SaveWorkspaceAs;
            context.AddFilter(Messages.SumericsWorkspace + " (*.sws)", "*.sws");
            context.ShowDialog();

            if (context.Accepted)
            {
                _kernel.SaveWorkspaceAsync(context.SelectedFile.FullName);
            }
        }

        public void Close()
        {
        }
    }
}
