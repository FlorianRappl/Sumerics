namespace Sumerics
{
    using Sumerics.Controls;
    using System;

    sealed class ConsoleProxy : IConsole
    {
        readonly ConsoleControl _console;

        public ConsoleProxy(ConsoleControl console)
        {
            _console = console;
        }

        public void Clear()
        {
            _console.Reset();
        }

        public void Execute(String query)
        {
            _console.InsertAndRun(query);
        }

        public void Execute(String query, String message)
        {
            _console.InsertAndRun(query, message);
        }
    }
}
