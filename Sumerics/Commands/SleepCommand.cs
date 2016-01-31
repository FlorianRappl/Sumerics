namespace Sumerics
{
	class SleepCommand : YCommand
	{
		public SleepCommand() : base(1, 1)
		{
		}

		public string Invocation(string arg)
		{
			return "sleep(" + arg + ")";
		}
	}
}
