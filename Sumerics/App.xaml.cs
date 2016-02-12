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
            foreach (var logger in Window.Container.All<ILogger>())
            {
                logger.Error(e.Exception);
            }

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
