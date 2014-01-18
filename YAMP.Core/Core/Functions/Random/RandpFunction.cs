using System;
using YAMP.Attributes;
using YAMP.Numerics;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("Generates a matrix with Poisson distributed random values. In probability theory and statistics, the Poisson distribution is a discrete probability distribution that expresses the probability of a given number of events occurring in a fixed interval of time and / or space if these events occur with a known average rate and independently of the time since the last event. The Poisson distribution can also be used for the number of events in other specified intervals such as distance, area or volume.")]
    [Kind(KindAttribute.FunctionKind.Random)]
    [Link("http://en.wikipedia.org/wiki/Poisson_distribution")]
    sealed class RandpFunction : YFunction
    {
        [Description("Generates one Poisson distributed random value with lambda set to 1.")]
        [Example("randp()", "Outputs a scalar that has been generated with respect to the distribution.")]
        public Double Invoke()
        {
            PoissonDistribution ran = new PoissonDistribution();
            ran.Lambda = 1.0;
            return ran.NextDouble();
        }

        [Description("Generates a n-by-n matrix with Poisson distributed random values with lambda set to 1.")]
        [Example("randp(3)", "Gives a 3x3 matrix with Poisson dist. rand. values.")]
        public Matrix Invoke(Int64 n)
        {
            return Invoke(n, n);
        }

        [Description("Generates a m-by-n matrix with Poisson distributed random values with lambda set to 1.")]
        [Example("randp(3, 1)", "Gives a 3x1 matrix with Poisson dist. rand. values.")]
        public Matrix Invoke(Int64 rows, Int64 cols)
        {
            return Invoke(rows, cols, 1.0);
        }

        [Description("Generates a m-by-n matrix with Poisson distributed random values with a custom lambda (mean).")]
        [Example("randp(3, 1, 10)", "Gives a 3x1 matrix with Poisson dist. rand. values with the mean mu set to 10.")]
        public Matrix Invoke(Int64 rows, Int64 cols, Double lambda)
        {
            PoissonDistribution ran = new PoissonDistribution();
            ran.Lambda = lambda;
            var n = (int)rows;
            var l = (int)cols;
            return Matrix.Zeros(n, l).ForEach(z => ran.NextDouble());
        }
    }
}
