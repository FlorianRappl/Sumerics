using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("The square root function raises the element to the power 1/2.")]
    [Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class SqrtFunction : YFunction
	{
        public Double Invoke(Double x)
        {
            return Math.Sqrt(x);
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Sqrt(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Sqrt);
        }
	}
}

