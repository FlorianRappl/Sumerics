using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("The inverse of the sin(x) function.")]
    [Kind(KindAttribute.FunctionKind.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Inverse_trigonometric_function")]
    sealed class ArcsinFunction : YFunction
    {
        public double Invoke(double x)
        {
            return Math.Asin(x);
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Arcsin(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Arcsin);
        }
    }
}
