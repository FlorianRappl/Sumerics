namespace Sumerics.Commands
{
    using System;

    sealed class ClearCommand : YCommand
    {
        public ClearCommand() : 
            base(0)
        {
        }

        public String Invocation()
        {
            return "clear()";
        }

        public String Invocation(params String[] Parameters)
        {
            var parameters = String.Join("\", \"", Parameters);
            return String.Format("clear(\"{0}\")", parameters);
        }
    }
}
