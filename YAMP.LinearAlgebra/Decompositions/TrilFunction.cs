using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
	[Kind("LinearAlgebra")]
    [Description("The function computes the lower triangle matrix of a given matrix.")]
    sealed class TrilFunction : YFunction
    {
        [Description("Given a square matrix the function computes the lower triangular matrix.")]
        [Example("tril(rand(4))", "Computes the lower triangle matrix of the given 4x4 random matrix, which is P * L from the P, L, U decomposition.")]
        public Matrix Invoke(Matrix M)
        {
            var lu = new LUDecomposition(M);
            return lu.Pivot * lu.L;
        }
    }
}
