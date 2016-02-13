namespace Sumerics.Dialogs
{
    using Sumerics.Views;
    using System;

    [DialogType(Dialog.Help)]
    sealed class HelpDialogHandler : IDialogHandler
    {
        readonly IComponents _container;

        public HelpDialogHandler(IComponents container)
        {
            _container = container;
        }

        public void Open(params Object[] parameters)
        {
            _container.Obtain<HelpWindow>().Show();
        }

        public void Close()
        {
            this.Close<HelpWindow>();
        }
    }
}
