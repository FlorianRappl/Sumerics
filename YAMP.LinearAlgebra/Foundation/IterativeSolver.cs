using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// The abstract base class for any iterative solver.
    /// </summary>
    public abstract class IterativeSolver : DirectSolver
    {
        #region ctor

        /// <summary>
        /// The matrix A that contains the description for a system of linear equations.
        /// </summary>
        /// <param name="A">The A in A * x = b.</param>
        public IterativeSolver(Matrix A)
        {
            this.A = A;
            MaxIterations = 5 * A.Columns * A.Rows;
            Tolerance = 1e-10;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the matrix A in A * x = b.
        /// </summary>
        public Matrix A { get; private set; }

        /// <summary>
        /// Gets or sets the maximum number of iterations.
        /// </summary>
        public int MaxIterations { get; set; }

        /// <summary>
        /// Gets or sets the starting vector x0.
        /// </summary>
        public Matrix X0 { get; set; }

        /// <summary>
        /// Gets or sets the tolerance level (when to stop the iteration?).
        /// </summary>
        public double Tolerance { get; set; }

        #endregion

        #region Helpers

        protected static Complex Dot(Matrix x, Matrix y)
        {
            var a = x.ToArray();
            var b = y.ToArray();
            var n = Math.Min(a.Length, b.Length);
            var s = Complex.Zero;

            for (int i = 0; i < n; i++)
                s += a[i].Conj() * b[i];

            return s;
        }

        #endregion
    }
}
