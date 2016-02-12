namespace Sumerics
{
    sealed class SumericsApp : IApplication
    {
        readonly IConsole _console;
        readonly IVisualizer _visualizer;
        readonly IKernel _kernel;
        readonly IDialogManager _dialogs;
        readonly ITabManager _tabs;
        readonly ISettings _settings;

        public SumericsApp(IConsole console, IVisualizer visualizer, IKernel kernel, IDialogManager dialogs, ITabManager tabs, ISettings settings)
        {
            _console = console;
            _visualizer = visualizer;
            _kernel = kernel;
            _dialogs = dialogs;
            _tabs = tabs;
        }

        public void Shutdown()
        {
            App.Current.Shutdown();
        }

        public IConsole Console
        {
            get { return _console; }
        }

        public IVisualizer Visualizer
        {
            get { return _visualizer; }
        }

        public IKernel Kernel
        {
            get { return _kernel; }
        }

        public ITabManager Tabs
        {
            get { return _tabs; }
        }

        public IDialogManager Dialog
        {
            get { return _dialogs; }
        }

        public ISettings Settings
        {
            get { return _settings; }
        }
    }
}
