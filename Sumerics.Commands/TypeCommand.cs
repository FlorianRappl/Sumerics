namespace Sumerics.Commands
{
    using System;

    sealed class TypeCommand : YCommand
    {
        public TypeCommand()
            : base(1, 1)
        {
        }

        public String Invocation(String filter)
        {
            return "type(" + filter + ")";
        }
    }
}