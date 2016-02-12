namespace Sumerics
{
    using Sumerics.Views;
    using System;
    using System.Windows;
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        readonly Container _container;

        public App()
        {
            _container = new Container();
            DispatcherUnhandledException += HandleUnhandledException;
        }

        void Application_Startup(Object sender, StartupEventArgs e)
        {
            var logger = new FileLogger();
            var kernel = new Kernel(logger);
            _container.Register(_container);
            _container.Register(logger);
            _container.Register(kernel);
            var mainWindow = new MainWindow(_container, kernel);
            var application = mainWindow.CreateApplication(kernel);
            _container.Register(application);
            var commands = mainWindow.CreateCommands();
            _container.Register(commands);
            mainWindow.Show();
        }

        void HandleUnhandledException(Object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            _container.All<ILogger>().ForEach(logger => logger.Error(e.Exception));
            OutputDialog.Show("Exception occurred", e.Exception.Message);
            e.Handled = true;
        }
    }
}
