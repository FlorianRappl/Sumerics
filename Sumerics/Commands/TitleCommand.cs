namespace Sumerics.Commands
{
    class TitleCommand:YCommand
    {
        public TitleCommand()
            : base(0, 2)
        {
        }

        public string Invocation()
        {
            return "title()";
        }

        public string Invocation(string parameter)
        {
            return "title(" + parameter + ")";
        }

        public string Invocation(string plot, string title)
        {
            return "title(" + plot + ", " + title + ")";
        }
    }
}