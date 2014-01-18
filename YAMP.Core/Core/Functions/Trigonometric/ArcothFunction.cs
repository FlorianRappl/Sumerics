using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("The inverse of the coth(x) function, which is cosh(x) / sinh(x).")]
    [Kind(KindAttribute.FunctionKind.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Inverse_hyperbolic_function")]
    sealed class ArcothFunction : YFunction
    {
        public double Invoke(double x)
        {
            return 0.5 * Math.Log((1.0 + x) / (x - 1.0));
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Arcoth(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Arcoth);
        }
    }
}
