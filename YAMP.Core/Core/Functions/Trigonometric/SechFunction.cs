using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("The standard sech(x) function. This is the hyperbolic secant.")]
    [Kind(KindAttribute.FunctionKind.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Hyperbolic_function")]
    sealed class SechFunction : YFunction
    {
        public double Invoke(double x)
        {
            return 1.0 / Math.Cosh(x);
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Sech(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Sech);
        }
    }
}
