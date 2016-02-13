namespace Sumerics.Dialogs
{
    using System;

    interface IDialogHandler
    {
        void Open(params Object[] parameters);

        void Close();
    }
}
