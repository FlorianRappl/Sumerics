using System;

namespace Sumerics
{
	class MvCommand : YCommand
	{
		public MvCommand() : base(2, 2)
		{
		}

		public string Invocation(string from, string to)
		{
			return "mv(@\"" + from + "\", @" + to + "\")";
		}
	}
}
