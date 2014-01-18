using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("Represents the heaviside step function.")]
    [Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class HeavisideFunction : YFunction
    {
        [Description("Returns 0 for values smaller or equal to 0, else 1.")]
        [Example("heaviside(3.1 - 4.7)", "Results in 0.")]
        [Example("heaviside(3 - 4)", "Results in 0.")]
        [Example("heaviside(3 - 2)", "Results in 1.")]
        public Int64 Invoke(Double x)
        {
            return x > 0 ? 1 : 0;
        }

        [Description("Returns 0 for values with real parts smaller or equal to 0, else 1.")]
        [Example("heaviside(1 + 3.1i - 4.7)", "Results in 0.")]
        [Example("heaviside(5 - 3.1i - 4.7)", "Results in 1.")]
        public Int64 Invoke(Complex z)
        {
            return z.Re > 0 ? 1 : 0;
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(z => z.Re > 0 ? 1 : 0);
        }
    }
}
