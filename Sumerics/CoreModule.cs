namespace Sumerics
{
    using Sumerics.Managers;
    using Sumerics.Properties;
    using Sumerics.Proxies;

    public class CoreModule : IModule
    {
        public void RegisterComponents(Services components)
        {
            components.Register<ILogger, FileLogger>();
            components.Register<IConsole, ConsoleProxy>();
            components.Register<ITabs, TabProxy>();
            components.Register<IVisualizer, VisualizerProxy>();
            components.Register<IKernel, Kernel>();
            components.Register<IDialogManager, DialogManager>();
            components.Register<ISettings>(new SettingsProxy(Settings.Default));
        }
    }
}
