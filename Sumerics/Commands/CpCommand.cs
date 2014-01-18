using System;

namespace Sumerics
{
	class CpCommand : YCommand
	{
		public CpCommand() : base(2, 2)
		{
		}

		public string Invocation(string from, string to)
		{
			return "cp(@\"" + from + "\", @" + to + "\")";
		}
	}
}
