namespace Sumerics.Commands
{
    using System;
    using System.Text;

    sealed class SaveCommand : YCommand
    {
        public SaveCommand() : 
            base(1)
        {
        }

        public String Invocation(String FileName)
        {
            return "save(@\"" + FileName + "\")";
        }

        public String Invocation(String FileName, params String[] Parameters)
        {
            var sb = new StringBuilder();
            sb.Append("save(@\"");
            sb.Append(FileName);
            sb.Append("\"");

            if (Parameters.Length > 0)
            {
                var parameters = String.Join("\", \"", Parameters);
                sb.Append(", \"").Append(parameters).Append("\"");
            }

            sb.Append(")");
            return sb.ToString();
        }
    }
}
