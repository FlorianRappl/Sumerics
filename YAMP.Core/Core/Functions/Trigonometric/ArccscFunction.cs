using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("The inverse of the csc(x) function.")]
    [Kind(KindAttribute.FunctionKind.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Inverse_trigonometric_function")]
    sealed class ArccscFunction : YFunction
    {
        public double Invoke(double x)
        {
            return Math.Asin(1.0 / x);
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Arccsc(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Arccsc);
        }
    }
}
