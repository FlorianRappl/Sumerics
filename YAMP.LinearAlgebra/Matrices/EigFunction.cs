using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
    [Kind("LinearAlgebra")]
	[Description("Computes the eigenvalues and eigenvectors of a given matrix.")]
    [Link("http://en.wikipedia.org/wiki/Eigendecomposition_of_a_matrix")]
    sealed class EigFunction : YFunction
	{
		[Description("Solves the eigenproblem of a matrix A and return a vector with all (and degenerate) eigenvalues.")]
        [Example("eig([1,2,3;4,5,6;7,8,9])", "Returns a vector with the three eigenvalues 16.11684, -1.11684 and 0 of this 3x3 matrix.")]
        [Example("[vals, vecs] = eig([1,2,3;4,5,6;7,8,9])", "Saves a vector with the three eigenvalues 16.11684, -1.11684 and 0 of this 3x3 matrix in the variable vals and a matrix containing the eigenvectors in the variable vecs.")]
        public EigResult Invoke(Matrix M)
		{
			var ev = new Eigenvalues(M);
            return new EigResult(ev);
		}

        public class EigResult
        {
            Eigenvalues ev;

            public EigResult(Eigenvalues ev)
            {
                this.ev = ev;
            }

            [Description("Gets the eigenvectors of the matrix stored in a matrix.")]
            public Matrix Vectors
            {
                get { return ev.GetV(); }
            }

            [Description("Gets the eigenvalues of the matrix stored in a block-diagonal matrix.")]
            public Matrix Diagonal
            {
                get { return ev.D; }
            }

            [Description("Gets the real parts of the eigenvalues of the matrix stored in a vector.")]
            public Matrix ReValues
            {
                get { return new Matrix(ev.RealEigenvalues); }
            }

            [Description("Gets the imaginary parts of the eigenvalues of the matrix stored in a vector.")]
            public Matrix ImValues
            {
                get { return new Matrix(ev.ImagEigenvalues); }
            }
        }
	}
}
