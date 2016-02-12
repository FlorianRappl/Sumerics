namespace Sumerics
{
    using System;
    using System.Windows;
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Application.Current.DispatcherUnhandledException += HandleUnhandledException;
        }

        void HandleUnhandledException(Object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Window.Container.All<ILogger>().ForEach(logger => logger.Error(e.Exception));
            OutputDialog.Show("Exception occurred", e.Exception.Message);
            e.Handled = true;
        }

        public static MainWindow Window
        {
            get;
            set;
        }
    }
}
