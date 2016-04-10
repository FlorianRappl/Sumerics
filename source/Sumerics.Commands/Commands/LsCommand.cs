namespace Sumerics.Commands
{
    using System;

	sealed class LsCommand : BaseCommand
	{
        public LsCommand() : 
            base(0, 1)
        {
        }

        public String Invocation()
        {
            return "ls()";
        }

		public String Invocation(String arg)
		{
			return "ls(\"" + arg + "\")";
		}
	}
}
