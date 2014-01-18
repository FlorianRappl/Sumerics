using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("Returns a logarithmically increased vector.")]
	[Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class LogspaceFunction : YFunction
	{
		[Description("Creates a vector with count elements ranging from a certain value to a certain value for the basis 10.")]
		[Example("logspace(2, 3, 5)", "Creates the vector [100, 177, 316, 562, 1000], i.e. start at 10^2 and end at 10^3 with number of elements 5.")]
        public Double[] Invoke(Double start, Double end, Int64 count)
		{
            return Invoke(start, end, count, 10.0);
		}

		[Description("Creates a vector with count elements ranging from a certain value to a certain value for an arbitrary basis.")]
		[Example("logspace(2, 6, 5, 2)", "Creates the vector [4, 8, 16, 32, 64], i.e. start at 2^2 and end at 2^6 with number of elements 5.")]
        public Double[] Invoke(Double start, Double end, Int64 count, Double basis)
        {
            if (count < 2)
                return new Double[0];
			
			var s = (end - start) / (count - 1);
            return Range.Create(start, end, s).ForEach(m => Math.Pow(basis, m));
		}
	}
}

