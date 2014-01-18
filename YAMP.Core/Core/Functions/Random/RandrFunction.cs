using System;
using YAMP.Attributes;
using YAMP.Numerics;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("Generates a matrix with Rayleigh distributed random values. In probability theory and statistics, the Rayleigh distribution is a continuous probability distribution. A Rayleigh distribution is often observed when the overall magnitude of a vector is related to its directional components. One example where the Rayleigh distribution naturally arises is when wind velocity is analyzed into its orthogonal 2-dimensional vector components. Assuming that the magnitude of each component is uncorrelated, normally distributed with equal variance, and zero mean, then the overall wind speed (vector magnitude) will be characterized by a Rayleigh distribution. A second example of the distribution arises in the case of random complex numbers whose real and imaginary components are independently and identically distributed Gaussian with equal variance and zero mean. In that case, the absolute value of the complex number is Rayleigh-distributed. The distribution is named after Lord Rayleigh.")]
    [Kind(KindAttribute.FunctionKind.Random)]
    [Link("http://en.wikipedia.org/wiki/Rayleigh_distribution")]
    sealed class RandrFunction : YFunction
    {
        [Description("Generates one Rayleigh distributed random value with sigma set to 1.")]
        [Example("randr()", "Outputs a scalar that has been generated with respect to the distribution.")]
        public Double Invoke()
        {
            RayleighDistribution ran = new RayleighDistribution();
            ran.Sigma = 1.0;
            return ran.NextDouble();
        }

        [Description("Generates a n-by-n matrix with Rayleigh distributed random values with sigma set to 1.")]
        [Example("randr(3)", "Gives a 3x3 matrix with Rayleigh dist. rand. values.")]
        public Matrix Invoke(Int64 n)
        {
            return Invoke(n, n);
        }

        [Description("Generates a m-by-n matrix with Rayleigh distributed random values with sigma set to 1.")]
        [Example("randr(3, 1)", "Gives a 3x1 matrix with Rayleigh dist. rand. values.")]
        public Matrix Invoke(Int64 rows, Int64 cols)
        {
            return Invoke(rows, cols, 1.0);
        }

        [Description("Generates a m-by-n matrix with Rayleigh distributed random values with a custom sigma (mode).")]
        [Example("randr(3, 1, 10)", "Gives a 3x1 matrix with Rayleigh dist. rand. values with the mode sigma set to 10.")]
        public Matrix Invoke(Int64 rows, Int64 cols, Double sigma)
        {
            RayleighDistribution ran = new RayleighDistribution();
            ran.Sigma = sigma;
            var n = (int)rows;
            var l = (int)cols;
            return Matrix.Zeros(n, l).ForEach(z => ran.NextDouble());
        }
    }
}
