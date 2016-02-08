namespace Sumerics.Commands
{
    using System;

    sealed class SetCommand : YCommand
    {
        public SetCommand()
            : base(2, 4)
        {
        }

        public String Invocation(String parameter0, String parameter1)
        {
            return "set(" + parameter0 + ", " + parameter1 + ")";
        }

        public String Invocation(String parameter0, String parameter1, String parameter2)
        {
            return "set(" + parameter0 + ", " + parameter1 + ", " + parameter2 + ")";
        }

        public String Invocation(String parameter0, String parameter1, String parameter2, String parameter3)
        {
            return "set(" + parameter0 + ", " + parameter1 + ", " + parameter2 + ", " + parameter3 + ")";
        }
    }
}
