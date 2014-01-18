using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("This is decimal logarithm, i.e. the logarithm with base 10.")]
    [Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class Log10Function : YFunction
    {
        public Complex Invoke(Double x)
        {
            return Math.Log10(x);
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Log(z, 10.0);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(z => Complex.Log(z, 10.0));
        }
    }
}
