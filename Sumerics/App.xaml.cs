namespace Sumerics
{
    using Sumerics.Commands;
    using System;
    using System.Windows;
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IApplication
    {
        readonly ConsoleProxy _console;
        readonly IContainer _container;

        public App()
        {
            Application.Current.DispatcherUnhandledException += HandleUnhandledException;
            _console = new ConsoleProxy();
            _container = CreateContainer(this);
        }

        static IContainer CreateContainer(IApplication application)
        {
            var container = new Container();
            container.Register(application);
            container.Register(new YCommandFactory(container));
            return container;
        }

        void HandleUnhandledException(Object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Core.LogError(e.Exception);
            OutputDialog.Show("Exception occurred", e.Exception.Message);
            e.Handled = true;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _container.Get<YCommandFactory>().RegisterCommands();
            base.OnStartup(e);
        }

        public static MainWindow Window
        {
            get;
            set;
        }

        public IConsole Console
        {
            get { return _console; }
        }
    }
}
