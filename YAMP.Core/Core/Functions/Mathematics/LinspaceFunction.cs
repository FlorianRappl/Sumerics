using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("Returns a uniformly increased vector.")]
    [Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class LinspaceFunction : YFunction
	{
		[Description("Creates a vector with count elements ranging from a certain value to a certain value.")]
		[Example("linspace(0, 10, 5)", "Creates the vector [0, 2.5, 5, 7.5, 10], i.e. stepping 2.5 and number of elements 5.")]
        public Double[] Invoke(Double from, Double to, Int64 count)
        {
            if (count < 2)
                return new Double[0];

            var step = (to - from) / (count - 1);
            return Range.Create(from, to, step).ToArray();
		}
	}
}

