using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("The inverse of the tan(x) function, which is sin(x) / cos(x).")]
    [Kind(KindAttribute.FunctionKind.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Inverse_trigonometric_function")]
    sealed class ArctanFunction : YFunction
    {
        public double Invoke(double x)
        {
            return Math.Atan(x);
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Arctan(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Arctan);
        }
    }
}
