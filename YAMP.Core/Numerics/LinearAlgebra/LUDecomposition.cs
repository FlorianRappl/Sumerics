using System;
using YAMP.Core;

namespace YAMP.Numerics
{
    /// <summary>
    /// LU Decomposition.
    /// For an m-by-n matrix A with m >= n, the LU decomposition is an m-by-n
    /// unit lower triangular matrix L, an n-by-n upper triangular matrix U,
    /// and a permutation vector piv of length m so that A(piv,:) = L*U.
    /// <code>
    /// If m is smaller than n, then L is m-by-m and U is m-by-n.
    /// </code>
    /// The LU decompostion with pivoting always exists, even if the matrix is
    /// singular, so the constructor will never fail.  The primary use of the
    /// LU decomposition is in the solution of square systems of simultaneous
    /// linear equations. This will fail if IsNonSingular() returns false.
    /// </summary>
    public sealed class LUDecomposition : DirectSolver
    {
        #region Members

        /// <summary>
        /// Array for internal storage of decomposition.
        /// </summary>
        Complex[][] LU;

        /// <summary>
        /// Row and column dimensions, and pivot sign.
        /// </summary>
        int m, n, pivsign;

        /// <summary>
        /// Internal storage of pivot vector.
        /// </summary>
        int[] piv;

        #endregion //  Class variables

        #region Constructor

        /// <summary>
        /// LU Decomposition
        /// </summary>
        /// <param name="A">Rectangular matrix</param>
        /// <returns>Structure to access L, U and piv.</returns>
        public LUDecomposition(Matrix A)
        {
            // Use a "left-looking", dot-product, Crout / Doolittle algorithm.
            LU = A.GetComplexMatrix();
            m = A.Rows;
            n = A.Columns;
            piv = new int[m];

            for (int i = 0; i < m; i++)
                piv[i] = i;
            
            pivsign = 1;
            var LUrowi = new Complex[0];
            var LUcolj = new Complex[m];

            // Outer loop.
            for (int j = 0; j < n; j++)
            {
                // Make a copy of the j-th column to localize references.
                for (int i = 0; i < m; i++)
                    LUcolj[i] = LU[i][j];

                // Apply previous transformations.
                for (int i = 0; i < m; i++)
                {
                    LUrowi = LU[i];

                    // Most of the time is spent in the following dot product.
                    var kmax = Math.Min(i, j);
                    var s = Complex.Zero;

                    for (int k = 0; k < kmax; k++)
                        s += LUrowi[k] * LUcolj[k];

					LUrowi[j] = LUcolj[i] -= s;
                }

                // Find pivot and exchange if necessary.
                var p = j;

                for (int i = j + 1; i < m; i++)
                {
                    if (LUcolj[i].Abs() > LUcolj[p].Abs())
                        p = i;
                }

                if (p != j)
                {
                    for (int k = 0; k < n; k++)
                    {
                        var t = LU[p][k]; 
                        LU[p][k] = LU[j][k]; 
                        LU[j][k] = t;
                    }
                    
                    var k2 = piv[p];
                    piv[p] = piv[j];
                    piv[j] = k2;
                    pivsign = -pivsign;
                }

                // Compute multipliers.

                if (j < m & LU[j][j] != 0.0)
                {
                    for (int i = j + 1; i < m; i++)
                        LU[i][j] = LU[i][j] / LU[j][j];
                }
            }
        }

        #endregion //  Constructor

        #region Public Properties

        /// <summary>
        /// Is the matrix nonsingular?
        /// </summary>
        /// <returns>true if U, and hence A, is nonsingular.</returns>
        public bool IsNonSingular
        {
            get
            {
                for (int j = 0; j < n; j++)
                {
                    if (LU[j][j] == Complex.Zero)
                        return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Return lower triangular factor
        /// </summary>
        /// <returns>L</returns>
        public Matrix L
        {
            get
            {
                var X = new Matrix(m, n);

                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (i > j)
                            X[i, j] = LU[i][j];
                        else if (i == j)
                            X[i, j] = 1;
                    }
                }

                return X;
            }
        }

        /// <summary>
        /// Return upper triangular factor
        /// </summary>
        /// <returns>U</returns>
        public Matrix U
        {
            get
            {
                var X = new Matrix(n, n);

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (i <= j)
                            X[i, j] = LU[i][j];
                    }
                }

                return X;
            }
        }

        /// <summary>
        /// Return pivot permutation vector
        /// </summary>
        /// <returns>piv</returns>
        public Matrix Pivot
        {
            get
            {
                var P = new Matrix(m, m);

                for (var i = 0; i < m; i++)
					P[i, piv[i]] = 1;
                
                return P;
            }
        }

        #endregion //  Public Properties

        #region Public Methods

        /// <summary>
        /// Determinant
        /// </summary>
        /// <returns>det(A)</returns>
        public Complex Determinant()
        {
            if (m != n)
                throw new YException("The argument needs to be a square matrix.");

            Complex d = new Complex(pivsign);

            for (int j = 0; j < n; j++)
                d = d * LU[j][j];

            return d;
        }

        /// <summary>
        /// Solve A*X = B
        /// </summary>
        /// <param name="b">A Matrix with as many rows as A and any number of columns.</param>
        /// <returns>X so that L*U*X = B(piv,:)</returns>
        public override Matrix Solve(Matrix b)
        {
            if (b.Rows != m)
                throw new YException("The number of rows of the vector (currently {1}) has to be equal to the number of columns in the matrix (currently {0}).", m, b.Rows);

            if (!this.IsNonSingular)
                throw new YException("Only non-singular matrices can be decomposed.");

            // Copy right hand side with pivoting
            var nx = b.Columns;
            var X = b.GetSubMatrix(piv, 0, nx).GetComplexMatrix();

            // Solve L*Y = B(piv,:)
            for (int k = 0; k < n; k++)
            {
                for (int i = k + 1; i < n; i++)
                {
                    for (int j = 0; j < nx; j++)
                        X[i][j] -= X[k][j] * LU[i][k];
                }
            }

            // Solve U*X = Y;
            for (int k = n - 1; k >= 0; k--)
            {
                for (int j = 0; j < nx; j++)
                    X[k][j] = X[k][j] / LU[k][k];

                for (int i = 0; i < k; i++)
                {
                    for (int j = 0; j < nx; j++)
                        X[i][j] -= X[k][j] * LU[i][k];
                }
            }

			return new Matrix(X, piv.Length, nx);
        }

        #endregion //  Public Methods
    }
}