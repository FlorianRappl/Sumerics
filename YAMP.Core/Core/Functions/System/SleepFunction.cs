using System;
using System.Threading;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("Enables the possibility to make a short break - computationally. Sets the computation thread on idle for a number of ms.")]
    [Kind(KindAttribute.FunctionKind.System)]
    sealed class SleepFunction : YFunction
	{
		[Description("Sets the computation thread on idle for the proposed time in milliseconds (ms).")]
		[Example("sleep(150)", "Sleeps for 150ms and outputs the real waiting time in ms.")]
		public Int64 Invoke(Int64 ms)
        {
			var start = Environment.TickCount;

			using (var blocking = new ManualResetEvent(false))
			{
                blocking.WaitOne((Int32)ms);
			}

			return Environment.TickCount - start;
		}
	}
}
