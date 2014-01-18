using System;

namespace Sumerics
{
    class HelpCommand : YCommand
    {
        public HelpCommand() : base(0, 1)
        {
        }

        public string Invocation()
        {
            return "help()";
        }

        public string Invocation(string method)
        {
            return "help(\"" + method + "\")";
        }
    }
}
