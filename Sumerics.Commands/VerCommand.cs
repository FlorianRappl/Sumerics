namespace Sumerics.Commands
{
    using System;

	sealed class VerCommand : YCommand
	{
		public VerCommand() : 
            base(0, 0)
		{
		}

		public String Invocation()
		{
			return "ver()";
		}
	}
}
