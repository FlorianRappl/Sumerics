using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("This is the exponential function, i.e. sum of n = 0 to infinity of x^n / n!.")]
    [Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class ExpFunction : YFunction
	{
        public Double Invoke(Double x)
        {
            return Math.Exp(x);
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Exp(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Exp);
        }
	}
}

