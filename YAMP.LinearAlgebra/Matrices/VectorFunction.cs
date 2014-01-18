using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
    [Kind("LinearAlgebra")]
    [Description("Creates a vector (a n-times-1 matrix) out of the given data.")]
    sealed class VectorFunction : YFunction
    {
        [Description("Creates a vector with the length of the given matrix. An mxn-matrix will be transformed to a vector with length m*n.")]
        [Example("vector([1,2;3,4])", "Creates the vector [1, 2, 3, 4] out of the given 2x2 matrix.")]
        public Complex[] Invoke(Matrix M)
        {
            return M.ToArray();
        }
    }
}
