using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("The standard csch(x) function. This is the hyperbolic cosecant.")]
    [Kind(KindAttribute.FunctionKind.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Hyperbolic_function")]
    sealed class CschFunction : YFunction
    {
        public double Invoke(double x)
        {
            return 1.0 / Math.Sinh(x);
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Csch(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Csch);
        }
    }
}
