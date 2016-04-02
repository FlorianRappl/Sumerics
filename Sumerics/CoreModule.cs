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
            var tabs = new TabProxy();
            var visualizer = new VisualizerProxy(console);
            var kernel = new Kernel(logger);
            var dialogs = new DialogManager(app);
            var settings = new SettingsProxy(Settings.Default);

            components.Register<ILogger>(logger);
            components.Register<IConsole>(console);
            components.Register<ITabs>(tabs);
            components.Register<IVisualizer>(visualizer);
            components.Register<IKernel>(kernel);
            components.Register<IDialogManager>(dialogs);
            components.Register<ISettings>(settings);
        }
    }
}
