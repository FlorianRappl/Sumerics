namespace Sumerics.Commands
{
    using System;

    sealed class ClsCommand : BaseCommand
    {
        readonly IConsole _console;

        public ClsCommand(IApplication app) : 
            base(0, 0)
        {
            _console = app.Get<IConsole>();
        }

        public String Invocation()
        {
            _console.Clear();
            return String.Empty;
        }
    }
}
