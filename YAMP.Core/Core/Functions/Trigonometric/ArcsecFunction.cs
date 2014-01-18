using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("The inverse of the sec(x) function.")]
    [Kind(KindAttribute.FunctionKind.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Inverse_trigonometric_function")]
    sealed class ArcsecFunction : YFunction
    {
        public double Invoke(double x)
        {
            return Math.Acos(1.0 / x);
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Arcsec(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Arcsec);
        }
    }
}
