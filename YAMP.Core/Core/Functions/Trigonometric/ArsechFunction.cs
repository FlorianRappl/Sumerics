using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("The inverse of the sech(x) function.")]
    [Kind(KindAttribute.FunctionKind.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Inverse_hyperbolic_function")]
    sealed class ArsechFunction : YFunction
    {
        public double Invoke(double x)
        {
            var xi = 1.0 / x;
            return Math.Log(xi + Math.Sqrt(xi + 1.0) * Math.Sqrt(xi - 1.0));
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Arsech(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Arsech);
        }
    }
}
