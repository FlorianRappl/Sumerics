using System;
using YAMP.Attributes;
using YAMP.Numerics;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("Generates a matrix with gamma distributed random values. In probability theory and statistics, the gamma distribution is a two-parameter family of continuous probability distributions.")]
    [Kind(KindAttribute.FunctionKind.Random)]
    [Link("http://en.wikipedia.org/wiki/Gamma_distribution")]
    sealed class RandgFunction : YFunction
    {
        [Description("Generates one gamma distributed random value with theta and k set to 1.")]
        [Example("randg()", "Outputs a scalar that has been generated with respect to the distribution.")]
        public Double Invoke()
        {
            GammaDistribution ran = new GammaDistribution();
            ran.Theta = 1.0;
            ran.Alpha = 1.0;
            return ran.NextDouble();
        }

        [Description("Generates a n-by-n matrix with gamma distributed random values with theta and k set to 1.")]
        [Example("randg(3)", "Gives a 3x3 matrix with gamma dist. rand. values.")]
        public Matrix Invoke(Int64 n)
        {
            return Invoke(n, n);
        }

        [Description("Generates a m-by-n matrix with gamma distributed random values with theta and k set to 1.")]
        [Example("randg(3, 1)", "Gives a 3x1 matrix with gamma dist. rand. values.")]
        public Matrix Invoke(Int64 rows, Int64 cols)
        {
            return Invoke(rows, cols, 1.0, 1.0);
        }

        [Description("Generates a m-by-n matrix with gamma distributed random values with a custom theta (scale) and k (shape) parameter.")]
        [Example("randg(3, 1, 10, 2.5)", "Gives a 3x1 matrix with gamma dist. rand. values around 10 with scale parameter theta set to 10 and shape parameter k set to 2.5.")]
        public Matrix Invoke(Int64 rows, Int64 cols, Double theta, Double k)
        {
            GammaDistribution ran = new GammaDistribution();
            ran.Theta = theta;
            ran.Alpha = k;
            var n = (int)rows;
            var l = (int)cols;
            return Matrix.Zeros(n, l).ForEach(z => ran.NextDouble());
        }
    }
}
