namespace Sumerics
{
    using System;

    public interface IVisualizer
    {
        void Dock();
        void Dock(Object context);
        void Undock();
        void Undock(Object context);
    }
}
