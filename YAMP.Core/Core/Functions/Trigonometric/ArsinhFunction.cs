using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("The inverse of the sinh(x) function.")]
    [Kind(KindAttribute.FunctionKind.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Inverse_hyperbolic_function")]
    sealed class ArsinhFunction : YFunction
    {
        public double Invoke(double x)
        {
            return Math.Log(x + Math.Sqrt((x * x) + 1.0));
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Arsinh(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Arsinh);
        }
    }
}
