using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("The standard csc(x) function. This is 1.0 over the sine or the hypotenuse over the opposite side.")]
    [Kind(KindAttribute.FunctionKind.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Trigonometric_functions")]
	sealed class CscFunction : YFunction
    {
        public double Invoke(double x)
        {
            return 1.0 / Math.Sin(x);
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Csc(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Csc);
        }
	}
}

