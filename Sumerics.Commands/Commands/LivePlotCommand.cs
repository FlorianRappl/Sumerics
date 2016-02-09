namespace Sumerics.Commands
{
    using System;
    using System.Text;

    sealed class LivePlotCommand : BaseCommand
    {
        public LivePlotCommand()
            : base(3)
        {
        }

        public String Invocation(params String[] Parameters)
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
