namespace Sumerics
{
    using System;

    class ExitCommand : YCommand
    {
        public ExitCommand() : base(0, 0)
        {
        }

        public String Invocation()
        {
            App.Current.Shutdown();
            return String.Empty;
        }
    }
}
