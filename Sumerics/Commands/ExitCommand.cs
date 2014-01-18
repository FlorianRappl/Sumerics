using System;

namespace Sumerics
{
    class ExitCommand : YCommand
    {
        public ExitCommand() : base(0, 0)
        {
        }

        public string Invocation()
        {
            App.Current.Shutdown();
            return string.Empty;
        }
    }
}
