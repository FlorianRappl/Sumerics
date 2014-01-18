using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
	[Kind("LinearAlgebra")]
	[Description("Performs the trace operation on the given matrix.")]
    [Link("http://en.wikipedia.org/wiki/Trace_(linear_algebra)")]
    sealed class TraceFunction : YFunction
	{
		[Description("Sums all elements on the diagonal.")]
		[Example("trace([1,2;3,4])", "Results in the value 5.")]
		public Complex Invoke(Matrix M)
		{
			return M.Trace();
		}
	}
}
