using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("Represents the ceil function to round up.")]
    [Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class CeilFunction : YFunction
    {
        public Double Invoke(Double x)
        {
            return Math.Ceiling(x);
        }

		public Complex Invoke(Complex z)
		{
            return Complex.Ceil(z);
		}

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Ceil);
        }
	}
}

