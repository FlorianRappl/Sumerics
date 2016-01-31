namespace Sumerics
{
    using System.Text;

    class TimeSeriesCommand : YCommand
    {
        public TimeSeriesCommand()
            : base(3)
        {
        }

        public string Invocation(string FunctionName, string NumberOfMeasurements, string TimeBetweenMeasurements)
        {
            return "timeseries(\"" + FunctionName + "\", " + NumberOfMeasurements + ", " + TimeBetweenMeasurements + ")";
        }

        public string Invocation(string FunctionName, string NumberOfMeasurements, string TimeBetweenMeasurements, params string[] Parameters)
        {
            var sb = new StringBuilder();
            sb.Append("timeseries(\"").Append(FunctionName);
            sb.Append("\", ").Append(NumberOfMeasurements);
            sb.Append(", ").Append(TimeBetweenMeasurements);

            if(Parameters.Length > 0)
                sb.Append(", ").Append(string.Join(", ", Parameters));

            sb.Append(")");
            return sb.ToString();
        }
    }
}
