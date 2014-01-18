using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// The abstract base class for every integrator algorithm.
    /// </summary>
    public abstract class Integrator
    {
        /// <summary>
        /// Creates a new integrator.
        /// </summary>
        /// <param name="y">The (y) vector to integrate.</param>
        public Integrator(double[] y)
        {
            Values = y;
        }

        /// <summary>
        /// Gets or sets the (y) values used by the integrator.
        /// </summary>
        public double[] Values { get; set; }

        /// <summary>
        /// Performs the integration with the values hold in Values and standard x values.
        /// </summary>
        /// <returns>The result of the integration.</returns>
        public virtual double Integrate()
        {
            var x = new Range(1, 1, Values.Length);
            return Integrate(x.ToArray());
        }

        /// <summary>
        /// Performs the integration with the values hold in Values and the given x values.
        /// </summary>
        /// <param name="x">The x values.</param>
        /// <returns>The result of the integration.</returns>
        public abstract double Integrate(double[] x);
    }
}
