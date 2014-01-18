using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("Represents the sign function.")]
    [Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class SignFunction : YFunction
    {
        [Description("Returns -1 for values smaller than 0, +1 for values greater than zero, else 0.")]
        [Example("sign(3 - 2)", "Results in +1.")]
        public Int64 Invoke(Int64 x)
        {
            return Math.Sign(x);
        }

        [Description("Returns -1 for values smaller than 0, +1 for values greater than zero, else 0.")]
        [Example("sign(3.2 - 4)", "Results in -1.")]
        public Double Invoke(Double x)
        {
            return Math.Sign(x);
        }

        [Description("Returns -1 for values smaller than 0, +1 for values greater than zero, else 0.")]
        [Example("sign(3i - 4)", "Results in -1.")]
        public Complex Invoke(Complex z)
        {
            return Complex.Sign(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Sign);
        }
    }
}
