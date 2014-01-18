using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// Abstract base class for various interpolation algorithms.
    /// </summary>
    public abstract class Interpolation
    {
        #region Members

        int np;
        double[] _xsamples;
        double[] _ysamples;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="xsamples">The x samples array.</param>
        /// <param name="ysamples">The y samples (function data at x) array.</param>
        public Interpolation(double[] xsamples, double[] ysamples)
        {
            _xsamples = xsamples;
            _ysamples = ysamples;
            np = Math.Min(xsamples.Length, ysamples.Length);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the array with the x samples.
        /// </summary>
        public double[] XSamples { get { return _xsamples; } }

        /// <summary>
        /// Gets the array with the y samples.
        /// </summary>
        public double[] YSamples { get { return _ysamples; } }

        /// <summary>
        /// Gets the number of interpolation points.
        /// </summary>
        public int Np { get { return np; } }

        #endregion

        #region Methods

        /// <summary>
        /// Uses the abstract Compute() methods to compute ALL values.
        /// </summary>
        /// <param name="x">The matrix with given x values.</param>
        /// <returns>The interpolated y values.</returns>
        public virtual Matrix ComputeValues(Matrix x)
        {
            return x.ForEach(m => ComputeValue(m.Re));
        }

        /// <summary>
        /// Computes an interpolated y-value for the given x-value.
        /// </summary>
        /// <param name="x">The x-value to search for a y-value.</param>
        /// <returns>The corresponding y-value.</returns>
        public abstract double ComputeValue(double x);

        /// <summary>
        /// Solves the system of linear equations for a tri-diagonal A in A * x = b.
        /// </summary>
        /// <param name="sub">The lower diagonal of A.</param>
        /// <param name="diag">The diagonal itself of A.</param>
        /// <param name="sup">The upper diagonal of A.</param>
        /// <param name="b">The vector b in A * x = b.</param>
        /// <param name="n">The length of the diagonal.</param>
        protected void SolveTridiag(double[] sub, double[] diag, double[] sup, ref double[] b, int n)
        {
            int i;

            for (i = 2; i <= n; i++)
            {
                sub[i] = sub[i] / diag[i - 1];
                diag[i] = diag[i] - sub[i] * sup[i - 1];
                b[i] = b[i] - sub[i] * b[i - 1];
            }

            b[n] = b[n] / diag[n];

            for (i = n - 1; i >= 1; i--)
                b[i] = (b[i] - sup[i] * b[i + 1]) / diag[i];
        }

        #endregion
    }
}
