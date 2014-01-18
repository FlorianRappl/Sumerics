using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("The inverse of the tanh(x) function, which is sinh(x) / cosh(x).")]
    [Kind(KindAttribute.FunctionKind.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Inverse_hyperbolic_function")]
    sealed class ArtanhFunction : YFunction
    {
        public double Invoke(double x)
        {
            return 0.5 * Math.Log((1.0 + x) / (1.0 - x));
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Artanh(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Artanh);
        }
    }
}
