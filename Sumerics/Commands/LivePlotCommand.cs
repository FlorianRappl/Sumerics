using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumerics.Commands
{
    class LivePlotCommand : YCommand
    {
        public LivePlotCommand()
            : base(3)
        {
        }

        public string Invocation(params string[] Parameters)
        {
            var sb = new StringBuilder();
            sb.Append("liveplot(\"").Append(Parameters[0]).Append("\"");

            for (int i = 1; i < Parameters.Length; i++)
            {
                sb.Append(", ").Append(Parameters[i]);
            }

            sb.Append(")");
            return sb.ToString();
        }
    }
}
