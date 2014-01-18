using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.Core.Functions
{
	[Description("Returns a boolean matrix to state if the given numbers are prime integers.")]
    [Kind(KindAttribute.FunctionKind.Logic)]
    [Link("http://en.wikipedia.org/wiki/Prime_number")]
    sealed class IsPrimeFunction : YFunction
    {
        public Boolean Invoke(Int64 x)
        {
            return Helpers.IsPrimeNumber(x);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(z => (z.Im == 0.0 && Math.Floor(z.Re) == z.Re && Helpers.IsPrimeNumber((Int64)z.Re)) ? 1 : 0);
        }
    }
}
