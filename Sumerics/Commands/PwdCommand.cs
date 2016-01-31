namespace Sumerics
{
	class PwdCommand : YCommand
	{
		public PwdCommand() : base(0, 0)
		{
		}

		public string Invocation()
		{
			return "pwd()";
		}
	}
}
