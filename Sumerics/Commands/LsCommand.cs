namespace Sumerics
{
	class LsCommand : YCommand
	{
        public LsCommand() : base(0, 1)
        {
        }

        public string Invocation()
        {
            return "ls()";
        }

		public string Invocation(string arg)
		{
			return "ls(\"" + arg + "\")";
		}
	}
}
