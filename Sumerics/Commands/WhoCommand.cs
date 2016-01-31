namespace Sumerics.Commands
{
    class WhoCommand:YCommand
    {
        public WhoCommand()
            : base(0, 1)
        {
        }

        public string Invocation()
        {
            return "who()";
        }

        public string Invocation(string name)
        {
            return "who(\"" + name + "\")";
        }
    }
}