namespace Sumerics
{
    using System;

    public interface IApplication
    {
        void Shutdown();

        void ChangeTab(Int32 selectedIndex);

        IConsole Console { get; }

        IVisualizer Visualizer { get; }

        IKernel Kernel { get; }

        void Open(Dialog value);
    }
}
