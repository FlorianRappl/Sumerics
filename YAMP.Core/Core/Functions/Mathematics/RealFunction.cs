using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("Keeps only the real part of the passed matrix or scalar value and omits the imaginary part. If z = x + i * y, then real(z) is just x.")]
	[Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class RealFunction : YFunction
	{
        public Double Invoke(Complex z)
		{
			return z.Re;
		}

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(z => z.Re);
        }
	}
}
