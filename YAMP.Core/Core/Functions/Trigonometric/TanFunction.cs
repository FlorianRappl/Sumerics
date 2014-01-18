using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("The standard tan(x) function, which is sin(x) / cos(x). This is the opposite over the adjacent side.")]
    [Kind(KindAttribute.FunctionKind.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Trigonometric_functions")]
    sealed class TanFunction : YFunction
    {
        public double Invoke(double x)
        {
            return Math.Tan(x);
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Tan(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Tan);
        }
    }
}
