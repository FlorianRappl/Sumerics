namespace Sumerics.Commands
{
    using System;

	sealed class CpCommand : YCommand
	{
		public CpCommand() : 
            base(2, 2)
		{
		}

        public String Invocation(String from, String to)
		{
			return "cp(@\"" + from + "\", @" + to + "\")";
		}
	}
}
