using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// The Newton polynomial interpolation method.
    /// </summary>
    public class NewtonInterpolation : Interpolation
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="vector">The Nx2 vector with the samples.</param>
        public NewtonInterpolation(double[] xsamples, double[] ysamples) : base(xsamples, ysamples)
        {
        }

        /// <summary>
        /// Computes a value f(t) at t.
        /// </summary>
        /// <param name="t">The t value.</param>
        /// <returns>Returns the interpolated y = f(t) value.</returns>
        public override double ComputeValue(double t)
        {
            double F, LN, XX, X = 1;
            int i, j, k;
            var x = XSamples;
            var y = YSamples;

            for (i = 1, LN = YSamples[0]; i < Np; i++)
            {
                X *= (t - XSamples[i]);

                for (j = 0, F = 0; j <= i; j++)
                {
                    for (k = 0, XX = 1; k <= i; k++)
                    {
                        if (k != j)
                            XX *= XSamples[j] - XSamples[k];
                    }

                    F += YSamples[j] / XX;
                }

                LN += X * F;
            }

            return LN;
        }
    }
}
