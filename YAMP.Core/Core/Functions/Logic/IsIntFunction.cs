using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("Returns a boolean matrix to state if the given values are integers.")]
    [Kind(KindAttribute.FunctionKind.Logic)]
    [Link("http://en.wikipedia.org/wiki/Integer")]
    sealed class IsIntFunction : YFunction
    {
        public Boolean Invoke(Double x)
        {
            return Math.Floor(x) == x;
        }

        public Boolean Invoke(Complex z)
        {
            return z.Im != 0 && Math.Floor(z.Re) == z.Re;
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(z => (z.Im != 0 && Math.Floor(z.Re) == z.Re) ? 1 : 0);
        }
    }
}
