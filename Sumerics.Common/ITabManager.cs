namespace Sumerics
{
    using System;

    public interface ITabManager
    {
        Int32 SelectedIndex { get; }

        void Change(Int32 selectedIndex);
    }
}
