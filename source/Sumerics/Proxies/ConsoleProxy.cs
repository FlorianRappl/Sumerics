namespace Sumerics.Proxies
{
    using Sumerics.Controls;
    using Sumerics.Views;
    using System;

    sealed class ConsoleProxy : IConsole
    {
        ConsoleControl _console;

        public ConsoleControl Console
        {
            get { return _console ?? (_console = GetConsole()); }
        }

        static ConsoleControl GetConsole()
        {
            var window = App.Current.MainWindow as MainWindow;

            if (window != null)
            {
                return window.MyConsole;
            }

            return null;
        }

        public void Clear()
        {
            Console.Reset();
        }

        public void Execute(String query)
        {
            Console.InsertAndRun(query);
        }

        public void Execute(String query, String message)
        {
            Console.InsertAndRun(query, message);
        }
    }
}
