using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// Represents the Trapez integration algorithm - a very simple rule for numerical integration.
    /// </summary>
    public class TrapezIntegrator : Integrator
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="y">The values to integrate.</param>
        public TrapezIntegrator(double[] y) : base(y)
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
        public override double Integrate(double[] x)
        {
            var y = Values;
            var n = N - 1;

            if (x.Length != y.Length)
                throw new Exception("The length of the given x-vector is different from the length of the y-vector.");

            var sum = (x[1] - x[0]) * y[0] + (x[n] - x[n - 1]) * y[n];

            for (var i = 1; i < n; i++)
                sum += (x[i + 1] - x[i - 1]) * y[i];

            return 0.5 * sum;
        }
    }
}
