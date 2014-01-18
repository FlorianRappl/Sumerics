using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("Represents the abs function.")]
	[Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class AbsFunction : YFunction
    {
        [Description("Gives the absolute value of the provided integer.")]
        [Example("abs(3 - 4)", "Results in 1.")]
		public Int64 Invoke(Int64 x)
		{
            return Math.Abs(x);
		}

        [Description("Gives the absolute value of the provided real number.")]
        [Example("abs(7.3)", "Results in 7.")]
        public Double Invoke(Double x)
        {
            return Math.Abs(x);
        }
        [Description("Gives the absolute value of the provided complex number.")]
        [Example("abs(3 + 4i)", "Results in 5.")]
        [Example("abs(7i)", "Results in 7.")]
        public Double Invoke(Complex z)
        {
            return z.Abs();
        }

        [Description("Gives the absolute value of the provided vector, or the determinant of the given matrix.")]
        [Example("abs([1, 2; 0, 4])", "Results in 4.")]
        [Example("abs([1, 2, 3])", "Results in the square root of 14.")]
        public Complex Invoke(Matrix M)
        {
            if (M.IsVector)
                return M.Abs();

            return M.Det();
        }
	}
}

