using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// The Neville polynom interpolation algorithm.
    /// </summary>
    public class NevilleInterpolator : Interpolation
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="vector">The Nx2 vector with the sample values.</param>
        public NevilleInterpolator(double[] xsamples, double[] ysamples) : base(xsamples, ysamples)
        {
        }

        /// <summary>
        /// Interpolates the y value for a given x (t) value.
        /// </summary>
        /// <param name="t">The x value for computing f(x) = y.</param>
        /// <returns>The interpolated value.</returns>
        public override double ComputeValue(double t)
        {
            var x = XSamples;
            var f = YSamples;
            var n = Np - 1;

            for (int m = 1; m <= n; m++)
            {
                for (int i = 0; i <= n - m; i++)
                    f[i] = ((t - x[i + m]) * f[i] + (x[i] - t) * f[i + 1]) / (x[i] - x[i + m]);
            }

            return f[0];
        }
    }
}
