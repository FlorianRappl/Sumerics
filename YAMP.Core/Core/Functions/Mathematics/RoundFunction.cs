using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("Represents the round function to round up or down to the nearest integer.")]
	[Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class RoundFunction : YFunction
    {
        public Double Invoke(Double x)
        {
            return Math.Round(x);
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Round(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Round);
        }
    }
}
