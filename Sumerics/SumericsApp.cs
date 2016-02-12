namespace Sumerics
{
    sealed class SumericsApp : IApplication
    {
        readonly Components _components;

        public SumericsApp()
        {
            _components = new Components();
            _components.Register<IApplication>(this);
        }

        public SumericsApp With<T>(T instance)
        {
            _components.Register<T>(instance);
            return this;
        }

        public void Shutdown()
        {
            App.Current.Shutdown();
        }

        public IConsole Console
        {
            get { return _components.Get<IConsole>(); }
        }

        public IVisualizer Visualizer
        {
            get { return _components.Get<IVisualizer>(); }
        }

        public IKernel Kernel
        {
            get { return _components.Get<IKernel>(); }
        }

        public ITabManager Tabs
        {
            get { return _components.Get<ITabManager>(); }
        }

        public IDialogManager Dialog
        {
            get { return _components.Get<IDialogManager>(); }
        }

        public ISettings Settings
        {
            get { return _components.Get<ISettings>(); }
        }

        public IComponents Components
        {
            get { return _components; }
        }
    }
}
