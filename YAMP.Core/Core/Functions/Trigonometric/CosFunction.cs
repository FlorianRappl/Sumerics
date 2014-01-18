using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("The standard cos(x) function. This is the adjacent over the hypotenuse side.")]
    [Kind(KindAttribute.FunctionKind.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Trigonometric_functions")]
    sealed class CosFunction : YFunction
	{
        public double Invoke(double x)
        {
            return Math.Cos(x);
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Cos(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Cos);
        }
	}
}

