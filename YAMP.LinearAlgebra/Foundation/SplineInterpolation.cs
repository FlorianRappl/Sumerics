using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// Interpolation with the spline method.
    /// </summary>
    public class SplineInterpolation : Interpolation
    {
        double[] a;
        double[] h;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="samples">The Nx2 matrix containing the sample data.</param>
        public SplineInterpolation(double[] xsamples, double[] ysamples) : base(xsamples, ysamples)
        {
            a = new double[Np];
            h = new double[Np];

            for (int i = 1; i < Np; i++)
                h[i - 1] = xsamples[i] - xsamples[i - 1];

            if (Np > 2)
            {
                double[] sub = new double[Np - 1];
                double[] diag = new double[Np - 1];
                double[] sup = new double[Np - 1];

                for (int i = 1; i < Np - 1; i++)
                {
                    int j = i - 1;
                    diag[j] = (h[j] + h[j + 1]) / 3;
                    sup[j] = h[j + 1] / 6;
                    sub[j] = h[j] / 6;
                    a[j] = (ysamples[i + 1] - ysamples[i]) / h[j + 1] - (ysamples[i] - ysamples[i - 1]) / h[j];

                }

                SolveTridiag(sub, diag, sup, ref a, Np - 2);
            }
        }

        /// <summary>
        /// Computes a specific interpolated value f(x).
        /// </summary>
        /// <param name="x">The value where to interpolate.</param>
        /// <returns>The computed value y = f(t).</returns>
        public override double ComputeValue(double x)
        {
            var xs = XSamples;
            var ys = YSamples;

            if (a.Length > 1)
            {
                int gap = 0;
                double previous = 0.0;

                for (int i = 0; i < a.Length - 1; i++)
                {
                    if (xs[i] < x && (i == 1 || xs[i]> previous))
                    {
                        previous = xs[i];
                        gap = i;
                    }
                }

                double x1 = x - previous;
                double x2 = h[gap] - x1;
                return ((-a[gap - 1] / 6 * (x2 + h[gap]) * x1 + ys[gap]) * x2 + (-a[gap] / 6 * (x1 + h[gap]) * x2 + ys[gap + 1]) * x1) / h[gap];
            }

            return 0;
        }
    }
}
