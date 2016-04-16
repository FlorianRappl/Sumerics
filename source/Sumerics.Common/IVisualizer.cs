namespace Sumerics
{
    using System;

    public interface IVisualizer
    {
        void Show(Object context);

        Object HideCurrent();

        void Dock(Object context);

        void DockLast();

        void Undock();

        void UndockAny(Object context);
    }
}
