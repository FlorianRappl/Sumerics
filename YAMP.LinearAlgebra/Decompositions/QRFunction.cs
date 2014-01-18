using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
    [Kind("LinearAlgebra")]
    [Link("http://en.wikipedia.org/wiki/QR_decomposition")]
	[Description("In linear algebra, a QR decomposition (also called a QR factorization) of a matrix is a decomposition of a matrix A into a product A=QR of an orthogonal matrix Q and an upper triangular matrix R. QR decomposition is often used to solve the linear least squares problem, and is the basis for a particular eigenvalue algorithm, the QR algorithm.")]
    sealed class QRFunction : YFunction
	{
		[Description("Any real square matrix A may be decomposed as A = QR, where Q is an orthogonal matrix (its columns are orthogonal unit vectors) and R is an upper triangular matrix (also called right triangular matrix). This generalizes to a complex square matrix A and a unitary matrix Q. If A is invertible, then the factorization is unique if we require that the diagonal elements of R are positive.")]
		[Example("qr([12, -51, 4; 6, 167, -68; -4, 24, -41])", "Computes the matrix Q of the given matrix.")]
		[Example("[Q, R] = qr([12, -51, 4; 6, 167, -68; -4, 24, -41])", "Computes the matrices Q and R and stores them in the variables Q and R.")]
        public QRResult Invoke(Matrix M)
		{
			var qr = QRDecomposition.Create(M);
			return new QRResult(qr);
		}

        public class QRResult
        {
            QRDecomposition qr;

            public QRResult(QRDecomposition qr)
            {
                this.qr = qr;
            }

            [Description("Gets the economy sized orthogonal factor matrix Q.")]
            public Matrix Q
            {
                get { return qr.Q; }
            }

            [Description("Gets the upper triangular factor matrix R.")]
            public Matrix R
            {
                get { return qr.R; }
            }
        }
	}
}
