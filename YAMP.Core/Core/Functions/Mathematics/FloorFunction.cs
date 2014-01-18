using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("Represents the floor function to round down.")]
    [Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class FloorFunction : YFunction
    {
        public Double Invoke(Double x)
        {
            return Math.Floor(x);
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Floor(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Floor);
        }
	}
}

