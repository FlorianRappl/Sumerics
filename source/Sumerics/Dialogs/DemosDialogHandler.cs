namespace Sumerics.Dialogs
{
    using Sumerics.Views;
    using System;

    [DialogType(Dialog.Demos)]
    sealed class DemosDialogHandler : IDialogHandler
    {
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
