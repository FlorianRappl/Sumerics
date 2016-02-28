namespace Sumerics
{
    using Sumerics.Resources;
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
            _app.RegisterAssemblies();
            var vm = new MainViewModel(_app.Components, _app.Kernel);
            var window = new MainWindow(vm);
            window.Show();
        }

        void HandleUnhandledException(Object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            _app.Components.All<ILogger>().ForEach(logger => logger.Error(e.Exception));
            OutputDialog.Show(Messages.Exception, e.Exception.Message);
            e.Handled = true;
        }
    }
}
