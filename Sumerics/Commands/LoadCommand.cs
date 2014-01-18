using System;
using System.Text;

namespace Sumerics
{
    class LoadCommand : YCommand
    {
        public LoadCommand() : base(1)
        {

        }

        public string Invocation(string FileName)
        {
            return "load(@\"" + FileName + "\")";
        }

        public string Invocation(string FileName, params string[] Parameters)
        {
            var sb = new StringBuilder();
            sb.Append("load(@\"");
            sb.Append(FileName);
            sb.Append("\"");

            if (Parameters.Length > 0)
                sb.Append(", \"").Append(string.Join("\", \"", Parameters)).Append("\"");

            sb.Append(")");
            return sb.ToString();
        }
    }
}
