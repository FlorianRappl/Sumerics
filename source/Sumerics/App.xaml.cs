﻿namespace Sumerics
{
    using Squirrel;
    using Sumerics.Properties;
    using Sumerics.Resources;
    using Sumerics.ViewModels;
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
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

            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Settings.Default.Language);

            DispatcherUnhandledException += HandleUnhandledException;
        }

        void Application_Startup(Object sender, StartupEventArgs e)
        {
            var updater = CheckForUpdates();
            _app.RegisterAssemblies();
            var vm = new MainViewModel(_app);
            vm.ShowWindow();
        }

        async Task CheckForUpdates()
        {
            try
            {
                using (var mgr = await UpdateManager.GitHubUpdateManager("https://github.com/FlorianRappl/Sumerics"))
                {
                    await mgr.UpdateApp();
                }
            }
            catch { }
        }

        void HandleUnhandledException(Object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            _app.All<ILogger>().ForEach(logger => logger.Error(e.Exception));
            var vm = new OutputViewModel { Title = Messages.Exception, Message = e.Exception.Message };
            vm.ShowDialog();
            e.Handled = true;
        }
    }
}
