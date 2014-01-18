using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("The standard sec(x) function. This is one over the cosine or the hypotenuse over the adjacent side.")]
    [Kind(KindAttribute.FunctionKind.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Trigonometric_functions")]
	sealed class SecFunction : YFunction
    {
        public double Invoke(double x)
        {
            return 1.0 / Math.Cos(x);
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Sec(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Sec);
        }
	}
}

