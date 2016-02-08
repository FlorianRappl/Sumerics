namespace Sumerics.Commands
{
    using System;

    sealed class ClsCommand : YCommand
    {
        readonly IApplication _app;

        public ClsCommand(IApplication app) : 
            base(0, 0)
        {
            _app = app;
        }

        public String Invocation()
        {
            _app.Console.Clear();
            return String.Empty;
        }
    }
}
