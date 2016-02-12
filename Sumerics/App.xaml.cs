namespace Sumerics
{
    using Sumerics.Commands;
    using Sumerics.Managers;
    using Sumerics.Properties;
    using Sumerics.Proxies;
    using Sumerics.ViewModels;
    using Sumerics.Views;
    using System;
    using System.Windows;
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        readonly SumericsApp _app;

        public App()
        {
            _app = new SumericsApp();
            DispatcherUnhandledException += HandleUnhandledException;
        }

        void Application_Startup(Object sender, StartupEventArgs e)
        {
            var logger = new FileLogger();
            var kernel = new Kernel(logger);
            var settings = new SettingsProxy(Settings.Default);
            var vm = new MainViewModel(_app.Components, kernel);
            var window = new MainWindow(vm);
            var console = new ConsoleProxy(window.MyConsole);
            var visualizer = new VisualizerProxy(vm, window.MyLastPlot);
            var dialogs = new DialogManager(_app.Components);
            var tabs = new TabManager(window.MainTabs);
            var commands = new CommandFactory(_app.Components);

            _app.With<ILogger>(logger)
                .With<IKernel>(kernel)
                .With<IConsole>(console)
                .With<IVisualizer>(visualizer)
                .With<IDialogManager>(dialogs)
                .With<ITabManager>(tabs)
                .With<ICommandFactory>(commands)
                .With<ISettings>(settings);

            commands.RegisterCommands();

            window.Show();
        }

        void HandleUnhandledException(Object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            _app.Components.All<ILogger>().ForEach(logger => logger.Error(e.Exception));
            OutputDialog.Show("Exception occurred", e.Exception.Message);
            e.Handled = true;
        }
    }
}
