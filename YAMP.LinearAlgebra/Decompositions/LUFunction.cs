using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
    [Kind("LinearAlgebra")]
    [Link("http://en.wikipedia.org/wiki/LU_decomposition")]
	[Description("In linear algebra, LU decomposition (also called LU factorization) factorizes a matrix as the product of a lower triangular matrix and an upper triangular matrix. The product sometimes includes a permutation matrix as well. The LU decomposition can be viewed as the matrix form of Gaussian elimination.")]
    sealed class LUFunction : YFunction
	{
		[Description("An LU decomposition is a decomposition of the form PA = LU, where L is a lower triangular matrix, P is a permutation matrix containing the pivot elements and U is an upper triangular matrix.")]
		[Example("lu([4, 3; 6, 3])", "Computes the LU-decomposition of the matrix and returns the lower matrix L.")]
		[Example("[L, U, P] = lu([4, 3; 6, 3])", "Computes the LU-decomposition of the matrix and returns the lower matrix L (saved in the variable L) and the upper matrix R (saved in the variable R). The permutation matrix is saved in the variable P.")]
        public LUResult Invoke(Matrix M)
		{
			var lu = new LUDecomposition(M);
			return new LUResult(lu);
		}

        public class LUResult
        {
            LUDecomposition lu;

            public LUResult(LUDecomposition lu)
            {
                this.lu = lu;
            }

            [Description("Gets the lower matrix L.")]
            public Matrix L
            {
                get { return lu.L; }
            }

            [Description("Gets the upper (right) matrix U.")]
            public Matrix U
            {
                get { return lu.U; }
            }

            [Description("Gets the permutation matrix P.")]
            public Matrix P
            {
                get { return lu.Pivot; }
            }
        }
	}
}
