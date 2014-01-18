using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("Returns a boolean matrix to state if the given values have imaginary parts.")]
	[Kind(KindAttribute.FunctionKind.Logic)]
    [Link("http://en.wikipedia.org/wiki/Complex_number")]
    sealed class IsComplexFunction : YFunction
    {
        public Boolean Invoke(Complex z)
        {
            return z.Im != 0.0;
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(z => z.Im != 0.0 ? 1 : 0);
        }
    }
}
