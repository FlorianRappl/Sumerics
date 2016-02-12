namespace Sumerics
{
    public interface IApplication
    {
        void Shutdown();

        ITabManager Tabs { get; }

        IConsole Console { get; }

        IVisualizer Visualizer { get; }

        IKernel Kernel { get; }

        IDialogManager Dialog { get; }

        ISettings Settings { get; }
    }
}
