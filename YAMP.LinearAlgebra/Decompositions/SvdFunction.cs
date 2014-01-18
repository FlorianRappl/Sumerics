using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
	[Kind("LinearAlgebra")]
    [Link("http://en.wikipedia.org/wiki/Singular_value_decomposition")]
	[Description("In linear algebra, the singular value decomposition (SVD) is a factorization of a real or complex matrix, with many useful applications in signal processing and statistics.")]
    sealed class SvdFunction : YFunction
	{
		[Description("Applications which employ the SVD include computing the pseudoinverse, least squares fitting of data, matrix approximation, and determining the rank, range and null space of a matrix.")]
		[Example("svd([1, 0, 0, 0, 2; 0, 0, 3, 0, 0; 0, 0, 0, 0, 0; 0, 4, 0, 0, 0])", "Computes the matrices Sigma (singular values), U (left-singular vectors) and V* (right-singular vectors) of the matrix.")]
		[Example("[S, U, V] = svd([1, 0, 0, 0, 2; 0, 0, 3, 0, 0; 0, 0, 0, 0, 0; 0, 4, 0, 0, 0])", "Computes the matrices Sigma, U and V* and stores them in the matrices S, U, V.")]
		public SvdResult Invoke(Matrix M)
		{
			var svd = new SingularValueDecomposition(M);
            return new SvdResult(svd);
		}

        public class SvdResult
        {
            SingularValueDecomposition svd;

            public SvdResult(SingularValueDecomposition svd)
            {
                this.svd = svd;
            }

            [Description("Gets the diagonal matrix of singular values (Sigma).")]
            public Matrix S
            {
                get { return svd.S; }
            }

            [Description("Gets the singular values D.")]
            public Matrix D
            {
                get { return new Matrix(svd.SingularValues); }
            }

            [Description("Gets the right singular vectors (V*).")]
            public Matrix V
            {
                get { return svd.GetV(); }
            }

            [Description("Gets the left singular vectors (U).")]
            public Matrix U
            {
                get { return svd.GetU(); }
            }
        }
	}
}
