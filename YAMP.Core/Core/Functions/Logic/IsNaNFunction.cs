using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("Returns a boolean matrix to state if the given numbers are proper numbers.")]
    [Kind(KindAttribute.FunctionKind.Logic)]
    [Link("http://en.wikipedia.org/wiki/NaN")]
    sealed class IsNaNFunction : YFunction
    {
        public Boolean Invoke(Double x)
        {
            return Double.IsNaN(x);
        }

        public Boolean Invoke(Complex z)
        {
            return Double.IsNaN(z.Re) || Double.IsNaN(z.Im);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(z => (Double.IsNaN(z.Re) || Double.IsNaN(z.Im)) ? 1 : 0);
        }
    }
}
