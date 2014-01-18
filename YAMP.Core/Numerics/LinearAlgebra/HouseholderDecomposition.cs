using System;
using YAMP.Core;

namespace YAMP.Numerics
{
    /// <summary>
    /// The Householder reflection is an implementation of a QR decomposition.
    /// This decomposition does not work for complex numbers.
    /// </summary>
    public sealed class HouseholderDecomposition : QRDecomposition
    {
        #region Members

        /// <summary>
        /// Array for internal storage of diagonal of R.
        /// </summary>
        //double[] Rdiag;
        Complex[] Rdiag;

        /// <summary>
        /// Array for internal storage of decomposition.
        /// </summary>
        Complex[][] QR;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new householder decomposition.
        /// </summary>
        /// <param name="A">The matrix to decompose.</param>
        public HouseholderDecomposition(Matrix A)
            : base(A)
        {
            QR = A.GetComplexMatrix();
            Rdiag = new Complex[n];

            // Main loop.
            for (int k = 0; k < n; k++)
            {
                var nrm = 0.0;

                for (int i = k; i < m; i++)
                    nrm = Helpers.Hypot(nrm, QR[i][k].Re);

                if (nrm != 0.0)
                {
                    // Form k-th Householder vector.

                    if (QR[k][k].Re < 0)
                        nrm = -nrm;

                    for (int i = k; i < m; i++)
                        QR[i][k] /= nrm;

                    QR[k][k] += Complex.One;

                    // Apply transformation to remaining columns.
                    for (int j = k + 1; j < n; j++)
                    {
                        var s = Complex.Zero;

                        for (int i = k; i < m; i++)
                            s += QR[i][k] * QR[i][j];

                        s = (-s) / QR[k][k];

                        for (int i = k; i < m; i++)
                            QR[i][j] += s * QR[i][k];
                    }
                }
                else
                    FullRank = false;

                Rdiag[k] = -nrm;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Return the upper triangular factor
        /// </summary>
        /// <returns>R</returns>
        public override Matrix R
        {
            get
            {
                var X = new Matrix(n, n);

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (i < j)
                            X[i, j] = QR[i][j];
                        else if (i == j)
                            X[i, j] = Rdiag[i];
                    }
                }

                return X;
            }
        }

        /// <summary>
        /// Generate and return the (economy-sized) orthogonal factor
        /// </summary>
        /// <returns>Q</returns>
        public override Matrix Q
        {
            get
            {
                var X = new Matrix(m, n);

                for (int k = n - 1; k >= 0; k--)
                {
                    for (int i = 0; i < m; i++)
                        X[i, k] = 0;

                    X[k, k] = 1;

                    for (int j = k; j < n; j++)
                    {
                        if (QR[k][k] != 0)
                        {
                            var s = Complex.Zero;

                            for (int i = k; i < m; i++)
                                s += QR[i - 1][k] * X[i, j].Re;

                            s = (-s) / QR[k][k];

                            for (int i = k; i < m; i++)
                                X[i, j] = X[i, j] + s * QR[i - 1][k];
                        }
                    }
                }

                return X;
            }
        }

        /// <summary>
        /// Return the Householder vectors
        /// </summary>
        /// <returns>Lower trapezoidal matrix whose columns define the reflections.</returns>
        public Matrix H
        {
            get
            {
                var X = new Matrix(m, n);

                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (i >= j)
                            X[i, j] = QR[i][j];
                    }
                }

                return X;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Least squares solution of A * X = B
        /// </summary>
        /// <param name="b">A Matrix with as many rows as A and any number of columns.</param>
        /// <returns>X that minimizes the two norm of Q*R*X-B.</returns>
        /// <exception cref="System.ArgumentException"> Matrix row dimensions must agree.</exception>
        /// <exception cref="System.SystemException"> Matrix is rank deficient.</exception>
        public override Matrix Solve(Matrix b)
        {
            if (b.Rows != m)
                throw new YException("The number of rows of the vector (currently {1}) has to be equal to the number of columns in the matrix (currently {0}).", m, b.Rows);

            if (!this.FullRank)
                throw new YException("Only non-singular matrices can be decomposed.");

            // Copy right hand side
            var nx = b.Columns;
            var X = b.GetComplexMatrix();

            // Compute Y = transpose(Q)*B
            for (int k = 0; k < n; k++)
            {
                for (int j = 0; j < nx; j++)
                {
                    var s = Complex.Zero;

                    for (int i = k; i < m; i++)
                        s += QR[i][k] * X[i][j];

                    s = (-s) / QR[k][k];

                    for (int i = k; i < m; i++)
                        X[i][j] += s * QR[i][k];
                }
            }

            // Solve R * X = Y;
            for (int k = n - 1; k >= 0; k--)
            {
                for (int j = 0; j < nx; j++)
                    X[k][j] /= Rdiag[k];

                for (int i = 0; i < k; i++)
                {
                    for (int j = 0; j < nx; j++)
                        X[i][j] -= X[k][j] * QR[i][k];
                }
            }

            return new Matrix(X, n, nx).GetSubMatrix(0, n, 0, nx);
        }

        #endregion
    }
}
