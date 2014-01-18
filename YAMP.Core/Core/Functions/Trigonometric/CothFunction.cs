using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("The standard coth(x) function, which is cosh(x) / sinh(x). This is the hyperbolic cotangent.")]
    [Kind(KindAttribute.FunctionKind.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Hyperbolic_function")]
    sealed class CothFunction : YFunction
    {
        public double Invoke(double x)
        {
            return 1.0 / Math.Tanh(x);
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Coth(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Coth);
        }
    }
}
