using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("The standard tanh(x) function, which is sinh(x) / cosh(x). This is the hyperbolic tangent.")]
    [Kind(KindAttribute.FunctionKind.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Hyperbolic_function")]
    sealed class TanhFunction : YFunction
    {
        public double Invoke(double x)
        {
            return Math.Tanh(x);
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Tanh(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Tanh);
        }
    }
}
