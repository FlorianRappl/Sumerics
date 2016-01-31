namespace Sumerics
{
	class CdCommand : YCommand
	{
		public CdCommand() : base(1, 1)
		{
		}

		public string Invocation(string arg)
		{
			return "cd(@\"" + arg + "\")";
		}
	}
}
