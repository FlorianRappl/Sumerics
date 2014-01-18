using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("Keeps only the imaginary part of the passed matrix or scalar value and omits the real part. If z = x + i * y, then imag(z) is just y.")]
    [Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class ImagFunction : YFunction
	{
        public Double Invoke(Complex z)
		{
			return z.Im;
		}

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(z => z.Im);
        }
	}
}
