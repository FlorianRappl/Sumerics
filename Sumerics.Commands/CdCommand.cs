namespace Sumerics.Commands
{
    using System;

	sealed class CdCommand : YCommand
	{
		public CdCommand() : 
            base(1, 1)
		{
		}

		public String Invocation(String arg)
		{
			return "cd(@\"" + arg + "\")";
		}
	}
}
