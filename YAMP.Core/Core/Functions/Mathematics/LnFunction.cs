using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("This is the natural logarithm, i.e. used to the basis of euler's number.")]
    [Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class LnFunction : YFunction
    {
        public Complex Invoke(Double x)
        {
            return Math.Log(x);
        }

        public Complex Invoke(Complex z)
        {
            return Complex.Ln(z);
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(Complex.Ln);
        }
    }
}

