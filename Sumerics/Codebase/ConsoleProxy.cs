namespace Sumerics
{
    using Sumerics.Controls;

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
    }
}
