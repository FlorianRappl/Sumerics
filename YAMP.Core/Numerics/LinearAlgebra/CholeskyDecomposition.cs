using System;
using YAMP;
using YAMP.Core;

namespace YAMP.Numerics
{
    /// <summary>
    /// Cholesky Decomposition.
    /// For a symmetric, positive definite matrix A, the Cholesky decomposition
    /// is an lower triangular matrix L so that A = L*L'.
    /// If the matrix is not symmetric or positive definite, the constructor
    /// returns a partial decomposition and sets an internal flag that may
    /// be queried by the isSPD() method.
    /// </summary>
    public sealed class CholeskyDecomposition : DirectSolver
    {
        #region Members

        /// <summary>
        /// Array for internal storage of decomposition.
        /// </summary>
        Complex[][] L;

        /// <summary>
        /// Row and column dimension (square matrix).
        /// </summary>
        int n;

        /// <summary>
        /// Symmetric and positive definite flag.
        /// </summary>
        bool isspd;

        #endregion //  Class variables

        #region Constructor

        /// <summary>
        /// Cholesky algorithm for symmetric and positive definite matrix.
        /// </summary>
        /// <param name="Arg">Square, symmetric matrix.</param>
        /// <returns>Structure to access L and isspd flag.</returns>
        public CholeskyDecomposition(Matrix Arg)
        {
            // Initialize.
            var A = Arg.GetComplexMatrix();
            n = Arg.Rows;
            L = new Complex[n][];

            for (int i = 0; i < n; i++)
                L[i] = new Complex[n];

            isspd = Arg.Columns == n;

            // Main loop.
            for (int i = 0; i < n; i++)
            {
                var Lrowi = L[i];
                var d = Complex.Zero;

                for (int j = 0; j < i; j++)
                {
                    var Lrowj = L[j];
                    var s = Complex.Zero;

                    for (int k = 0; k < j; k++)
                        s += Lrowi[k] * Lrowj[k].Conj();

                    s = (A[i][j] - s) / L[j][j];
                    Lrowi[j] = s;
                    d += s * s.Conj();
                    isspd = isspd && (A[j][i] == A[i][j]);
                }

                d = A[i][i] - d;
                isspd = isspd & (d.Abs() > 0.0);
                L[i][i] = Complex.Sqrt(d);

                for (int k = i + 1; k < n; k++)
                    L[i][k] = Complex.Zero;
            }
        }

        #endregion //  Constructor

        #region Properties
        /// <summary>
        /// Is the matrix symmetric and positive definite?
        /// </summary>
        /// <returns>true if A is symmetric and positive definite.</returns>
        public bool SPD
        {
            get
            {
                return isspd;
            }
        }
        #endregion   // Public Properties

        #region Public Methods

        /// <summary>
        /// Return triangular factor.
        /// </summary>
        /// <returns>L</returns>

        public Matrix GetL()
        {
            return new Matrix(L, n, n);
        }

        /// <summary>Solve A*X = B</summary>
        /// <param name="b">  A Matrix with as many rows as A and any number of columns.
        /// </param>
        /// <returns>     X so that L*L'*X = B
        /// </returns>
        /// <exception cref="System.ArgumentException">  Matrix row dimensions must agree.
        /// </exception>
        /// <exception cref="System.SystemException"> Matrix is not symmetric positive definite.
        /// </exception>

        public override Matrix Solve(Matrix b)
        {
            if (b.Rows != n)
                throw new YException("The number of rows of the vector (currently {1}) has to be equal to the number of columns in the matrix (currently {0}).", n, b.Rows);

            if (!isspd)
                throw new YException("The matrix needs to be symmetric positive definite.");

            // Copy right hand side.
            var X = b.GetComplexMatrix();
            int nx = b.Columns;

            // Solve L*Y = B;
            for (int k = 0; k < n; k++)
            {
                for (int i = k + 1; i < n; i++)
                {
                    for (int j = 0; j < nx; j++)
                        X[i][j] -= X[k][j] * L[i][k];
                }

                for (int j = 0; j < nx; j++)
                    X[k][j] /= L[k][k];
            }

            // Solve L'*X = Y;
            for (int k = n - 1; k >= 0; k--)
            {
                for (int j = 0; j < nx; j++)
                    X[k][j] /= L[k][k];

                for (int i = 0; i < k; i++)
                {
                    for (int j = 0; j < nx; j++)
                        X[i][j] -= X[k][j] * L[k][i];
                }
            }

            return new Matrix(X, n, nx);
        }

        #endregion //  Public Methods
    }
}