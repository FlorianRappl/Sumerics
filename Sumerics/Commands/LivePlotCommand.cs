namespace Sumerics.Commands
{
    using System.Text;

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

            for (var i = 1; i < Parameters.Length; i++)
            {
                sb.Append(", ").Append(Parameters[i]);
            }

            sb.Append(")");
            return sb.ToString();
        }
    }
}
