using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumerics.Commands
{
    class TypeCommand:YCommand
    {
        public TypeCommand()
            : base(1, 1)
        {
        }

        public string Invocation(string filter)
        {
            return "type(" + filter + ")";
        }
    }
}