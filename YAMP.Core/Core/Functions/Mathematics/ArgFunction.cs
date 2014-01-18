using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("This is a function operating on complex numbers (visualised as a flat plane). It gives the angle between the line joining the point to the origin and the positive real axis known as an argument of the point (that is, the angle between the half-lines of the position vector representing the number and the positive real axis).")]
    [Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class ArgFunction : YFunction
    {
        public Double Invoke(Complex z)
        {
            return z.Arg();
        }

        public Matrix Invoke(Matrix M)
        {
            return M.ForEach(z => z.Arg());
        }
    }
}
