namespace Sumerics.Dialogs
{
    using Sumerics.Views;
    using System;

    [DialogType(Dialog.Demos)]
    sealed class DemosDialogHandler : IDialogHandler
    {
        readonly IComponents _container;

        public DemosDialogHandler(IComponents container)
        {
            _container = container;
        }

        public void Open(params Object[] parameters)
        {
            this.Show<DemoBrowser>();
        }

        public void Close()
        {
            this.Close<DemoBrowser>();
        }
    }
}
