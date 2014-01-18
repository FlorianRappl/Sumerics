using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumerics.Commands
{
    class SetCommand:YCommand
    {
        public SetCommand()
            : base(2, 4)
        {
        }

        public string Invocation(string parameter0, string parameter1)
        {
            return "set(" + parameter0 + ", " + parameter1 + ")";
        }

        public string Invocation(string parameter0, string parameter1, string parameter2)
        {
            return "set(" + parameter0 + ", " + parameter1 + ", " + parameter2 + ")";
        }

        public string Invocation(string parameter0, string parameter1, string parameter2, string parameter3)
        {
            return "set(" + parameter0 + ", " + parameter1 + ", " + parameter2 + ", " + parameter3 + ")";
        }
    }
}
