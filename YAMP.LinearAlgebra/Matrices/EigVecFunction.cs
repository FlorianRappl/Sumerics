using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
    [Kind("LinearAlgebra")]
    [Description("Computes the eigenvectors of a given matrix.")]
    sealed class EigVecFunction : YFunction
    {
        [Description("Solves the eigenproblem of a matrix A and return a matrix with all (and degenerate) eigenvectors.")]
        [Example("eigvec([1,2,3;4,5,6;7,8,9])", "Returns a 3x3 matrix with the three eigenvectors of this 3x3 matrix.")]
        public Matrix Invoke(Matrix M)
        {
            var ev = new Eigenvalues(M);
            return ev.GetV();
        }
    }
}
