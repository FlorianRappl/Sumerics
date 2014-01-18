using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// Basic class for a Conjugant Gradient solver.
    /// </summary>
    class CGSolver : IterativeSolver
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="A">The matrix A for which to solve.</param>
        public CGSolver(Matrix A) : base(A)
        {
        }

        /// <summary>
        /// Solves a system of linear equation using the given matrix A.
        /// </summary>
        /// <param name="b">The source vector b, i.e. A * x = b.</param>
        /// <returns>The solution vector x.</returns>
        public override Matrix Solve(Matrix b)
        {
            Matrix x = X0;

            if (x == null)
                x = new Matrix(b.Rows, b.Columns);
            else if (x.Columns != b.Columns || x.Rows != b.Rows)
                throw new Exception("The matrix dimensions do not fit.");

            var r = b - A * x;
            var p = r.Copy();
            var l = Math.Max(A.Length, MaxIterations);
            var rsold = Dot(r, r);

            for(var i = 1; i < l; i++)
            {
                var Ap = A * p;
                var alpha = rsold / Dot(p, Ap);
                x = x + alpha * p;
                r = r - alpha * Ap;
                var rsnew = Dot(r, r);

                if (rsnew.Abs() < Tolerance)
                    break;

                p = r + rsnew / rsold * p;
                rsold = rsnew;
            }

            return x;
        }
    }
}
