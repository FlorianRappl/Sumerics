using System;
using YAMP.Core;

namespace YAMP.Numerics
{
    /// <summary>
    /// The Givens rotation is an implementation of a QR decomposition.
    /// This decomposition also works for complex numbers.
    /// </summary>
    public sealed class GivensDecomposition : QRDecomposition
    {
        #region Members

        Matrix q;
        Matrix r;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new Givens decomposition.
        /// </summary>
        /// <param name="A">The matrix to decompose.</param>
        public GivensDecomposition(Matrix A)
            : base(A)
        {
            var Q = Matrix.One(m);
            var R = A.Copy();

            // Main loop.
            for (int j = 0; j < n - 1; j++)
            {
                for (int i = m - 1; i > j; i--)
                {
                    var a = R[i - 1, j];
                    var b = R[i, j];
                    var G = Matrix.One(m);

                    var beta = Complex.Sqrt(a * a + b * b);
                    var s = -b / beta;
                    var c = a / beta;

                    G[i - 1, i - 1] = c.Conj();
                    G[i - 1, i] = -s.Conj();
                    G[i, i - 1] = s;
                    G[i, i] = c;

                    R = G * R;
                    Q = Q * G.Adj();
                }
            }

            for (int j = 0; j < n; j++)
            {
                if (R[j, j] == Complex.Zero)
                    FullRank = false;
            }

            r = R;
            q = Q;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Generate and return the (economy-sized) orthogonal factor
        /// </summary>
        /// <returns>Q</returns>
        public override Matrix Q
        {
            get { return q; }
        }

        /// <summary>
        /// Return the upper triangular factor
        /// </summary>
        /// <returns>R</returns>
        public override Matrix R
        {
            get { return r; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Least squares solution of A * X = B
        /// </summary>
        /// <param name="B">A Matrix with as many rows as A and any number of columns.</param>
        /// <returns>X that minimizes the two norm of Q*R*X-B.</returns>
        /// <exception cref="System.ArgumentException"> Matrix row dimensions must agree.</exception>
        /// <exception cref="System.SystemException"> Matrix is rank deficient.</exception>
        public override Matrix Solve(Matrix b)
        {
            if (b.Rows != m)
                throw new YException("The number of rows of the vector (currently {1}) has to be equal to the number of columns in the matrix (currently {0}).", m, b.Rows);

            if (!this.FullRank)
                throw new YException("Only non-singular matrices can be decomposed.");

            var cols = b.Columns;

            var r = Q.Trans() * b;
            var x = new Matrix(n, cols);

            for (int j = n - 1; j >= 0; j--)
            {
                for (int i = 0; i < cols; i++)
                {
                    var o = Complex.Zero;

                    for (int k = j + 1; k <= n; k++)
                        o += R[j, k] * x[k, i];

                    x[j, i] = (r[j, i] - o) / R[j, j];   
                }
            }

            return x;
        }

        #endregion
    }
}
