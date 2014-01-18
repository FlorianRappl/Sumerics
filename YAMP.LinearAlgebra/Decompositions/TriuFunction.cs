using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
    [Kind("LinearAlgebra")]
    [Description("The function computes the upper triangle matrix of a given matrix.")]
    sealed class TriuFunction : YFunction
    {
        [Description("Given a square matrix the function computes the upper triangular matrix.")]
        [Example("triu(rand(4))", "Computes the upper triangle matrix of the given 4x4 random matrix.")]
        public Matrix Invoke(Matrix M)
        {
            var lu = new LUDecomposition(M);
            return lu.U;
        }
    }
}
