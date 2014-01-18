using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("The standard sinh(x) function. This is the hyperbolic sine.")]
    [Kind(KindAttribute.FunctionKind.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Hyperbolic_function")]
    sealed class SinhFunction : YFunction
    {
        public double Invoke(double x)
        {
            return Math.Sinh(x);
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Sinh(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Sinh);
        }
    }
}
