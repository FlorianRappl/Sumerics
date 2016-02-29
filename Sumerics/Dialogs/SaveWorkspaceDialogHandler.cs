namespace Sumerics.Dialogs
{
    using Sumerics.Resources;
    using Sumerics.ViewModels;
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
            var context = new SaveFileViewModel(Environment.CurrentDirectory);
            context.AddFilter(Messages.SumericsWorkspace + " (*.sws)", "*.sws");
            var dialog = _container.Obtain<SaveFileWindow>();
            dialog.DataContext = context;
            dialog.Title = Messages.SaveWorkspaceAs;
            dialog.ShowDialog();

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
