using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("Conjugates the given complex number to transform it from x + iy to x - iy.")]
    [Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class ConjFunction : YFunction
    {
        public Complex Invoke(Complex z)
        {
            return z.Conj();
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(z => z.Conj());
        }
    }
}
