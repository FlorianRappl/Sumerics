using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.Core.Functions
{
	[Description("Represents the factorial function, which is used for the ! operator and integer expressions.")]
	[Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class FacFunction : YFunction
	{
		public Double Invoke(Int64 z)
		{
            return Helpers.Factorial((int)z);
		}

        public Double Invoke(Double z)
        {
            return Math.Exp(Gamma.LogGamma(z + 1.0));
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Factorial);
        }
	}
}

