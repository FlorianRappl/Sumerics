using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("The inverse of the cos(x) function.")]
    [Kind(KindAttribute.FunctionKind.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Inverse_trigonometric_function")]
    sealed class ArccosFunction : YFunction
    {
        public double Invoke(double x)
        {
            return Math.Acos(x);
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Arccos(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Arccos);
        }
    }
}
