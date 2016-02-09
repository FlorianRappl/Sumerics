namespace Sumerics.Commands
{
    using System;

    sealed class ExitCommand : BaseCommand
    {
        readonly IApplication _app;

        public ExitCommand(IApplication app) : 
            base(0, 0)
        {
            _app = app;
        }

        public String Invocation()
        {
            _app.Shutdown();
            return String.Empty;
        }
    }
}
