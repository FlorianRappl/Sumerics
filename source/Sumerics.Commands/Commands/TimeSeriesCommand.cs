namespace Sumerics.Commands
{
    using System;
    using System.Text;

    sealed class TimeSeriesCommand : BaseCommand
    {
        public TimeSeriesCommand()
            : base(3)
        {
        }

        public String Invocation(String FunctionName, String NumberOfMeasurements, String TimeBetweenMeasurements)
        {
            return "timeseries(\"" + FunctionName + "\", " + NumberOfMeasurements + ", " + TimeBetweenMeasurements + ")";
        }

        public String Invocation(String FunctionName, String NumberOfMeasurements, String TimeBetweenMeasurements, params String[] Parameters)
        {
            var sb = new StringBuilder();
            sb.Append("timeseries(\"").Append(FunctionName);
            sb.Append("\", ").Append(NumberOfMeasurements);
            sb.Append(", ").Append(TimeBetweenMeasurements);

            if (Parameters.Length > 0)
            {
                var parameters = String.Join(", ", Parameters);
                sb.Append(", ").Append(parameters);
            }

            sb.Append(")");
            return sb.ToString();
        }
    }
}
