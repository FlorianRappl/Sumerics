namespace Sumerics
{
    sealed class ConsoleProxy : IConsole
    {
        public void Clear()
        {
            App.Window.MyConsole.Reset();
        }
    }
}
