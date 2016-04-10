namespace Sumerics
{
    using System;

    public interface ITabs
    {
        Int32 SelectedIndex { get; }

        void Change(Int32 selectedIndex);
    }
}
