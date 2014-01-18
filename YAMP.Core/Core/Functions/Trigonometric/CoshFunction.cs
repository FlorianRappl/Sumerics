using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("The standard cosh(x) function. This is the hyperbolic cosine.")]
    [Kind(KindAttribute.FunctionKind.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Hyperbolic_function")]
    sealed class CoshFunction : YFunction
    {
        public double Invoke(double x)
        {
            return Math.Cosh(x);
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Cosh(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Cosh);
        }
    }
}
