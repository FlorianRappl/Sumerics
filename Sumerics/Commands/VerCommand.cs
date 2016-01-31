namespace Sumerics
{
	class VerCommand : YCommand
	{
		public VerCommand() : base(0, 0)
		{
		}

		public string Invocation()
		{
			return "ver()";
		}
	}
}
