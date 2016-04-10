namespace Sumerics.Commands
{
    using System;

	sealed class MvCommand : BaseCommand
	{
		public MvCommand() : 
            base(2, 2)
		{
		}

        public String Invocation(String from, String to)
		{
			return "mv(@\"" + from + "\", @" + to + "\")";
		}
	}
}
