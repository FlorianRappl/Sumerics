using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("The inverse of the csch(x) function.")]
    [Kind(KindAttribute.FunctionKind.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Inverse_hyperbolic_function")]
    sealed class ArcschFunction : YFunction
    {
        public double Invoke(double x)
        {
            return Math.Log(1.0 / x + Math.Sqrt(1.0 / (x * x) + 1.0));
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Arcsch(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Arcsch);
        }
    }
}
