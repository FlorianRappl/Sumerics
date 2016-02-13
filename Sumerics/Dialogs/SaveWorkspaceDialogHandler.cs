namespace Sumerics.Dialogs
{
    using Sumerics.Views;
    using System;

    [DialogType(Dialog.SaveWorkspace)]
    sealed class SaveWorkspaceDialogHandler : IDialogHandler
    {
        readonly IComponents _container;
        readonly IKernel _kernel;

        public SaveWorkspaceDialogHandler(IComponents container, IKernel kernel)
        {
            _container = container;
            _kernel = kernel;
        }

        public void Open(params Object[] parameters)
        {
            var dialog = _container.Obtain<SaveFileWindow>();
            dialog.Title = "Save workspace as ...";
            dialog.AddFilter("Sumerics workspace (*.sws)", "*.sws");
            dialog.ShowDialog();

            if (dialog.Accepted)
            {
                _kernel.SaveWorkspaceAsync(dialog.SelectedFile);
            }
        }

        public void Close()
        {
        }
    }
}
