using System;
using YAMP.Attributes;
using YAMP.Numerics;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("Generates a matrix with binomial distributed random values. In probability theory and statistics, the binomial distribution is the discrete probability distribution of the number of successes in a sequence of n independent yes/no experiments, each of which yields success with probability p. Such a success/failure experiment is also called a Bernoulli experiment or Bernoulli trial; when n = 1, the binomial distribution is a Bernoulli distribution. The binomial distribution is the basis for the popular binomial test of statistical significance.")]
    [Kind(KindAttribute.FunctionKind.Random)]
    [Link("http://en.wikipedia.org/wiki/Binomial_distribution")]
    sealed class RandbFunction : YFunction
    {
        [Description("Generates one binomial distributed random value with p set to 0.5 and n set to 1.")]
        [Example("randw()", "Outputs a scalar that has been generated with respect to the distribution.")]
        public Double Invoke()
        {
            BinomialDistribution ran = new BinomialDistribution();
            ran.Alpha = 0.5;
            ran.Beta = 1;
            return ran.NextDouble();
        }

        [Description("Generates a n-by-n matrix with binomial distributed random values with p set to 0.5 and n set to 1.")]
        [Example("randw(3)", "Gives a 3x3 matrix with binomial dist. rand. values.")]
        public Matrix Invoke(Int64 n)
        {
            return Invoke(n, n);
        }

        [Description("Generates a m-by-n matrix with binomial distributed random values with p set to 0.5 and n set to 1.")]
        [Example("randw(3, 1)", "Gives a 3x1 matrix with binomial dist. rand. values.")]
        public Matrix Invoke(Int64 rows, Int64 cols)
        {
            return Invoke(rows, cols, 0.5, 1);
        }

        [Description("Generates a m-by-n matrix with binomial distributed random values with a custom p (scale) and n (shape) parameter.")]
        [Example("randw(3, 1, 0.5, 20)", "Gives a 3x1 matrix with binomial dist. rand. values around 10 with probability parameter p set to 0.5 and trials parameter n set to 20.")]
        public Matrix Invoke(Int64 rows, Int64 cols, Double p, Int64 n)
        {
            BinomialDistribution ran = new BinomialDistribution();
            ran.Alpha = p;
            ran.Beta = (int)n;
            var k = (int)rows;
            var l = (int)cols;
            var m = new Matrix(k, l);
            return m.ForEach(z => ran.NextDouble());
        }
    }
}
