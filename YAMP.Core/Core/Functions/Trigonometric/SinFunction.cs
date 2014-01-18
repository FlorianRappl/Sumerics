using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("The standard sin(x) function. This is the opposite over the hypotenuse side.")]
    [Kind(KindAttribute.FunctionKind.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Sine")]
    sealed class SinFunction : YFunction
    {
        public double Invoke(double x)
        {
            return Math.Sin(x);
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Sin(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Sin);
        }
	}
}

