namespace Sumerics
{
    using System;

    public interface IDialogManager
    {
        void Open(Dialog value, params Object[] parameters);

        void Close(Dialog value);
    }
}
