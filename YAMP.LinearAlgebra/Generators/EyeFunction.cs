using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
    [Kind("LinearAlgebra")]
    [Description("Generates an identity matrix. In linear algebra, the identity matrix or unit matrix of size n is the n x n square matrix with ones on the main diagonal and zeros elsewhere.")]
    [Link("http://en.wikipedia.org/wiki/Identity_matrix")]
    sealed class EyeFunction : YFunction
    {
        [Description("Generates an n-dimensional identity matrix.")]
        [Example("eye(5)", "Returns a 5x5 matrix with 1 on the diagonal and 0 else.")]
        public Matrix Invoke(Int64 n)
        {
            return Matrix.One((int)n);
        }

        [Description("Generates an n x m-dimensional identity matrix.")]
        [Example("eye(5, 3)", "Returns a 5x3 matrix with 1 on the diagonal and 0 else.")]
        public Matrix Invoke(Int64 n, Int64 m)
        {
            return Matrix.One((int)n, (int)m);
        }
    }
}
