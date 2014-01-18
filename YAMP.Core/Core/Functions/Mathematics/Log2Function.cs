using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("This is binary logarithm, i.e. the logarithm with base 2.")]
    [Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class Log2Function : YFunction
    {
        public Complex Invoke(Double x)
        {
            return Math.Log(x, 2.0);
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Log(z, 2.0);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(z => Complex.Log(z, 2.0));
        }
    }
}
