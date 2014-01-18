using System;
using YAMP.Attributes;
using YAMP.Numerics;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("Generates a matrix with Laplace distributed random values. In probability theory and statistics, the Laplace distribution is a continuous probability distribution named after Pierre-Simon Laplace. It is also sometimes called the double exponential distribution, because it can be thought of as two exponential distributions (with an additional location parameter) spliced together back-to-back, but the term double exponential distribution is also sometimes used to refer to the Gumbel distribution. The difference between two independent identically distributed exponential random variables is governed by a Laplace distribution, as is a Brownian motion evaluated at an exponentially distributed random time. Increments of Laplace motion or a variance gamma process evaluated over the time scale also have a Laplace distribution.")]
    [Kind(KindAttribute.FunctionKind.Random)]
    [Link("http://en.wikipedia.org/wiki/Laplace_distribution")]
    sealed class RandlFunction : YFunction
    {
        [Description("Generates one Laplace distributed random value with mu set to 0 and b set to 1.")]
        [Example("randl()", "Outputs a scalar that has been generated with respect to the distribution.")]
        public Double Invoke()
        {
            LaplaceDistribution ran = new LaplaceDistribution();
            ran.Mu = 0.0;
            ran.Alpha = 1.0;
            return ran.NextDouble();
        }

        [Description("Generates a n-by-n matrix with Laplace distributed random values with mu set to 0 and b set to 1.")]
        [Example("randl(3)", "Gives a 3x3 matrix with Laplace dist. rand. values.")]
        public Matrix Invoke(Int64 n)
        {
            return Invoke(n, n);
        }

        [Description("Generates a m-by-n matrix with Laplace distributed random values with mu set to 0 and b set to 1.")]
        [Example("randl(3, 1)", "Gives a 3x1 matrix with Laplace dist. rand. values.")]
        public Matrix Invoke(Int64 rows, Int64 cols)
        {
            return Invoke(rows, cols, 0.0, 1.0);
        }

        [Description("Generates a m-by-n matrix with Laplace distributed random values with a custom mu (mean) and b (variance) parameter.")]
        [Example("randl(3, 1, 10, 2.5)", "Gives a 3x1 matrix with Laplace dist. rand. values around 10 with the mean mu set to 10 and variance parameter b set to 2.5. The variance scales with b^2, such that the standard deviation scales with b.")]
        public Matrix Invoke(Int64 rows, Int64 cols, Double mu, Double b)
        {
            LaplaceDistribution ran = new LaplaceDistribution();
            ran.Mu = mu;
            ran.Alpha = b;
            var n = (int)rows;
            var l = (int)cols;
            return Matrix.Zeros(n, l).ForEach(z => ran.NextDouble());
        }
    }
}
