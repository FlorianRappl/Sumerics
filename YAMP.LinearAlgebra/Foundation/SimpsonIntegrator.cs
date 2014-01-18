using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// Represents a specific algorithm for integration - Simpson's rule.
    /// </summary>
    public class SimpsonIntegrator : Integrator
    {
        /// <summary>
        /// Creates a new Simpson integrator.
        /// </summary>
        /// <param name="y">The values to integrate.</param>
        public SimpsonIntegrator(double[] y) : base(y)
        {
            N = y.Length;
        }

        /// <summary>
        /// Gets the number of values.
        /// </summary>
        public int N
        {
            get;
            private set;
        }

        /// <summary>
        /// Performs the integration.
        /// </summary>
        /// <param name="x">The x values.</param>
        /// <returns>The result of the integration.</returns>
        public override Double Integrate(double[] x)
        {
            var y = Values;
            var n = N - 2;

            if (x.Length != y.Length)
                throw new Exception("The length of the given x-vector is different from the length of the y-vector.");

            var sum = 0.0;

            for (var i = 0; i < n; i += 2)
                sum += (x[i + 2] - x[i]) * (y[i] + 4.0 * y[i + 1] + y[i + 2]);

            return sum / 6.0;
        }
    }
}
