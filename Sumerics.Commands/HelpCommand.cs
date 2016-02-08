namespace Sumerics.Commands
{
    using System;

    sealed class HelpCommand : YCommand
    {
        public HelpCommand() :
            base(0, 1)
        {
        }

        public String Invocation()
        {
            return "help()";
        }

        public String Invocation(String method)
        {
            return "help(\"" + method + "\")";
        }
    }
}
