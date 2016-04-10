namespace Sumerics
{
    using Sumerics.Managers;
    using Sumerics.Properties;
    using Sumerics.Proxies;

    public class CoreModule : IModule
    {
        public void RegisterComponents(Services components)
        {
            var app = components.Get<IApplication>();
            var logger = new FileLogger();
            var console = new ConsoleProxy();

            components.Register<ILogger>(logger);
            components.Register<IConsole>(console);
            components.Register<ITabs>(new TabProxy());
            components.Register<IVisualizer>(new VisualizerProxy(console));
            components.Register<IKernel>(new Kernel(logger));
            components.Register<IDialogManager>(new DialogManager(app));
            components.Register<ISettings>(new SettingsProxy(Settings.Default));
        }
    }
}
