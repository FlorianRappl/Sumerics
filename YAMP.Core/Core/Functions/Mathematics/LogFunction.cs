using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("This is the more general logarithm, i.e. by default the natural logarith, but with the ability to define the basis.")]
    [Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class LogFunction : YFunction
	{
        [Description("Evaluates the natural logarithm of the given real value.")]
        [Example("log(e)", "The natural logarithm for Euler's constant is 1.")]
        public Double Invoke(Double x)
        {
            return Math.Log(x);
        }

        [Description("Evaluates the custom logarithm of the given real value.")]
        [Example("log(16, 2)", "The binary logarithm for the argument 16 is 4.")]
        public Double Invoke(Double x, Double b)
        {
            return Math.Log(x, b);
        }

        [Description("Evaluates the natural logarithm of the given complex value.")]
        [Example("log(2-i)", "Takes the natural logarithm of 2-i.")]
        public Complex Invoke(Complex z)
        {
            return Complex.Log(z);
        }

        [Description("Evaluates the custom logarithm of the given complex value.")]
        [Example("log(8i, 2)", "The binary logarithm for the argument 8i is about 2 + pi / 2 * i.")]
        public Complex Invoke(Complex z, Double b)
        {
            return Complex.Log(z, b);
        }

        [Description("Evaluates the natural logarithm of the values of the given matrix.")]
        [Example("log(randi(10, 0, 1000))", "Generates a 10x10 matrix with integer random numbers between 0 and 1000. Returns a matrix with their natural logarithms.")]
        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(c => Complex.Log(c));
        }

        [Description("Evaluates the custom logarithm of the values of the given matrix.")]
        [Example("log(randi(10, 0, 1000), 10)", "Generates a 10x10 matrix with integer random numbers between 0 and 1000. Returns a matrix that contains the magnitudes of their values.")]
        public Matrix Invoke(Matrix M, Double b)
        {
            return M.ForEach(c => Complex.Log(c, b));
        }
	}
}

