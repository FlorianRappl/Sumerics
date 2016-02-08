namespace Sumerics.Commands
{
    using System;

	sealed class PwdCommand : YCommand
	{
		public PwdCommand() : 
            base(0, 0)
		{
		}

        public String Invocation()
		{
			return "pwd()";
		}
	}
}
