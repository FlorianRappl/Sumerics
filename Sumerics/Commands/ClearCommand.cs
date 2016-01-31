namespace Sumerics
{
    class ClearCommand : YCommand
    {
        public ClearCommand() : base(0)
        {
        }

        public string Invocation()
        {
            return "clear()";
        }

        public string Invocation(params string[] Parameters)
        {
            return string.Format("clear(\"{0}\")", string.Join("\", \"", Parameters));
        }
    }
}
