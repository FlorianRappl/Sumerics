namespace Sumerics.Commands
{
    using System;

    sealed class TitleCommand : BaseCommand
    {
        public TitleCommand()
            : base(0, 2)
        {
        }

        public String Invocation()
        {
            return "title()";
        }

        public String Invocation(String parameter)
        {
            return "title(" + parameter + ")";
        }

        public String Invocation(String plot, String title)
        {
            return "title(" + plot + ", " + title + ")";
        }
    }
}