namespace Sumerics
{
    class ClsCommand : YCommand
    {
        public ClsCommand() : base(0, 0)
        {
        }

        public string Invocation()
        {
            App.Window.MyConsole.Reset();
            return string.Empty;
        }
    }
}
