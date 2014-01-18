using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("The standard cot(x) function, which is cos(x) / sin(x). This is the adjacent over the opposite side.")]
    [Kind(KindAttribute.FunctionKind.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Trigonometric_functions")]
    sealed class CotFunction : YFunction
    {
        public double Invoke(double x)
        {
            return 1.0 / Math.Tan(x);
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Cot(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Cot);
        }
    }
}
