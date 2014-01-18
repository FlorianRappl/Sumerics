using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("Returns a boolean matrix to state if the given numbers are infinite.")]
    [Kind(KindAttribute.FunctionKind.Logic)]
    [Link("http://en.wikipedia.org/wiki/Infinity")]
    sealed class IsInfiniteFunction : YFunction
    {
        public Boolean Invoke(Double x)
        {
            return Double.IsInfinity(x);
        }

        public Boolean Invoke(Complex z)
        {
            return Double.IsInfinity(z.Re) || Double.IsInfinity(z.Im);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(z => (Double.IsInfinity(z.Re) || Double.IsInfinity(z.Im)) ? 1 : 0);
        }
    }
}
