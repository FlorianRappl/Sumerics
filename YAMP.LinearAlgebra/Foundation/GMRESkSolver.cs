using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// Basic class for a GMRES(k) (with restarts) solver.
    /// </summary>
    public class GMRESkSolver : IterativeSolver
    {
        #region Members

        int i = 0;
        Matrix H;
        Matrix V;
        Matrix gamma;
        Matrix c;
        Matrix s;

        #endregion

        #region ctor

        /// <summary>
        /// Creates the class for a GMRES(k) solver.
        /// </summary>
        /// <param name="A">The matrix A to solve.</param>
        public GMRESkSolver(Matrix A) : this(A, false)
        {
        }

        /// <summary>
        /// Creates the class for a GMRES(k) solver.
        /// </summary>
        /// <param name="A">The matrix A to consider as system of linear equations.</param>
        /// <param name="restart">Should restarts be executed?</param>
        public GMRESkSolver(Matrix A, bool restart) : base(A)
        {
            MaxIterations = A.Length;

            if(restart)
                Restart = MaxIterations / 10;
            else //No Restart
                Restart = MaxIterations;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets if restarts should be performed.
        /// </summary>
        public int Restart { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Solves the system of linear equations.
        /// </summary>
        /// <param name="b">The vector b in A * x = b.</param>
        /// <returns>The solution vector x.</returns>
        public override Matrix Solve(Matrix b)
        {
            var k = Restart;
            Matrix x;

            if (X0 == null)
                X0 = new Matrix(b.Rows, b.Columns);
            else if (X0.Columns != b.Columns || X0.Rows != b.Rows)
                throw new Exception("The matrix dimensions do not fit.");

            H = new Matrix(k + 1, k);
            V = new Matrix(X0.Rows, k);
            c = new Matrix(k - 1, 1);
            s = new Matrix(k - 1, 1);
            gamma = new Matrix(k + 1, 1);
            var converged = false;

            do
            {
                var j = 0;
                x = X0.Copy();
                var r0 = b - A * x;
                var beta = r0.Abs();

                H.Clear();
                V.Clear();
                gamma.Clear();

                gamma[0, 0] = beta;
                c.Clear();
                s.Clear();

                V.SetColumnVector(0, r0 / gamma[0, 0]);

                if (beta < Tolerance)
                    break;

                do
                {
                    var Avj = A * V.GetColumnVector(j);
                    var sum = new Matrix(Avj.Rows, Avj.Columns);

                    for (int m = 0; m <= j; m++)
                    {
                        var w = V.GetColumnVector(m);
                        H[m, j] = Dot(w, Avj);
                        sum += H[m, j] * w;
                    }

                    var wj = Avj - sum;
                    H[j + 1, j] = wj.Abs();
                    Rotate(j);

                    if (H[j + 1, j].Abs() == 0.0)
                    {
                        converged = true;
                        break;
                    }

                    V.SetColumnVector(j + 1, wj / H[j + 1, j]);
                    beta = gamma[j + 1, 0].Abs();

                    if (beta < Tolerance)
                    {
                        converged = true;
                        break;
                    }

                    j++;
                    i++;
                }
                while (j < k);

                var y = new Matrix(j, 1);

                for (int l = j - 1; l >= 0; l--)
                {
                    var sum = Complex.Zero;

                    for (int m = l; m < j; m++)
                        sum += H[l, m] * y[m, 0];

                    y[l, 0] = (gamma[l, 0] - sum) / H[l, l];
                }

                for (int l = 0; l < j; l++)
                    x += y[l, 0] * V.GetColumnVector(l);

                if (converged)
                    break;

                X0 = x;
            }
            while (i < MaxIterations);

            return x;
        }

        void Rotate(int j)
        {
            for (int i = 0; i < j; i++)
            {
                var v1 = H[i, j];
                var v2 = H[i + 1, j];
                H[i, j] = c[i, 1].Conj() * v1 + s[i, 1].Conj() * v2;
                H[i + 1, j] = c[i, 1] * v2 - s[i, 1] * v1;
            }

            var beta = Math.Sqrt(H[j, j].Re * H[j, j].Re + H[j, j].Im * H[j, j].Im + 
                H[j + 1, j].Re * H[j + 1, j].Re + H[j + 1, j].Im * H[j + 1, j].Im);

            s[j, 0] = H[j + 1, j] / beta;
            c[j, 0] = H[j, j] / beta;
            H[j, j] = beta;

            gamma[j + 1, 0] = -(s[j, 0] * gamma[j, 0]);
            gamma[j + 0, 0] = c[j, 0].Conj() * gamma[j, 0];
        }

        #endregion
    }
}
