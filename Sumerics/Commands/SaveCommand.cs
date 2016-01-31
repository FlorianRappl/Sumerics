namespace Sumerics
{
    using System.Text;

    class SaveCommand : YCommand
    {
        public SaveCommand() : base(1)
        {
        }

        public string Invocation(string FileName)
        {
            return "save(@\"" + FileName + "\")";
        }

        public string Invocation(string FileName, params string[] Parameters)
        {
            var sb = new StringBuilder();
            sb.Append("save(@\"");
            sb.Append(FileName);
            sb.Append("\"");

            if (Parameters.Length > 0)
            {
                sb.Append(", \"").Append(string.Join("\", \"", Parameters)).Append("\"");
            }

            sb.Append(")");
            return sb.ToString();
        }
    }
}
