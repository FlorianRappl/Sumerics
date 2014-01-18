using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("Represents the round function to round towards zero.")]
    [Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class FixFunction : YFunction
    {
        public Int64 Invoke(Double x)
        {
            return (Int64)x;
        }

        public Int64 Invoke(Complex z)
        {
            return (Int64)z;
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(z => (Int64)z);
        }
	}
}
