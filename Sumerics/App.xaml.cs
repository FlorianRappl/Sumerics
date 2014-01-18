using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Sumerics
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Application.Current.DispatcherUnhandledException += HandleUnhandledException;
        }

        void HandleUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Core.LogError(e.Exception);
            OutputDialog.Show("Exception occurred", e.Exception.Message);
            e.Handled = true;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            YCommand.RegisterCommands();
            base.OnStartup(e);
        }

        public static MainWindow Window
        {
            get;
            set;
        }
    }
}
