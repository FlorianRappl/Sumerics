namespace Sumerics.Dialogs
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    sealed class DialogTypeAttribute : Attribute
    {
        public DialogTypeAttribute(Dialog type)
        {
            Type = type;
        }

        public Dialog Type
        {
            get;
            private set;
        }
    }
}
