using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumerics
{
    class ClsCommand : YCommand
    {
        public ClsCommand() : base(0, 0)
        {
        }

        public string Invocation()
        {
            App.Window.MyConsole.Reset();
            return string.Empty;
        }
    }
}
