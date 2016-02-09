namespace Sumerics.Commands
{
    using System;

    sealed class WhoCommand : BaseCommand
    {
        public WhoCommand()
            : base(0, 1)
        {
        }

        public String Invocation()
        {
            return "who()";
        }

        public String Invocation(String name)
        {
            return "who(\"" + name + "\")";
        }
    }
}