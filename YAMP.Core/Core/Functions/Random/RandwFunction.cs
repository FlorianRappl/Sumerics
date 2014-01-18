using System;
using YAMP.Attributes;
using YAMP.Numerics;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("Generates a matrix with Weibull distributed random values. In probability theory and statistics, the Weibull distribution is a continuous probability distribution.")]
    [Kind(KindAttribute.FunctionKind.Random)]
    [Link("http://en.wikipedia.org/wiki/Weibull_distribution")]
    sealed class RandwFunction : YFunction
    {
        [Description("Generates one Weibull distributed random value with lambda and k set to 1.")]
        [Example("randw()", "Outputs a scalar that has been generated with respect to the distribution.")]
        public Double Invoke()
        {
            WeibullDistribution ran = new WeibullDistribution();
            ran.Lambda = 1.0;
            ran.Alpha = 1.0;
            return ran.NextDouble();
        }

        [Description("Generates a n-by-n matrix with Weibull distributed random values with lambda and k set to 1.")]
        [Example("randw(3)", "Gives a 3x3 matrix with Weibull dist. rand. values.")]
        public Matrix Invoke(Int64 n)
        {
            return Invoke(n, n);
        }

        [Description("Generates a m-by-n matrix with Weibull distributed random values with lambda and k set to 1.")]
        [Example("randw(3, 1)", "Gives a 3x1 matrix with Weibull dist. rand. values.")]
        public Matrix Invoke(Int64 rows, Int64 cols)
        {
            return Invoke(rows, cols, 1.0, 1.0);
        }

        [Description("Generates a m-by-n matrix with Weibull distributed random values with a custom lambda (scale) and k (shape) parameter.")]
        [Example("randw(3, 1, 10, 2.5)", "Gives a 3x1 matrix with Weibull dist. rand. values around 10 with scale parameter lambda set to 10 and shape parameter k set to 2.5.")]
        public Matrix Invoke(Int64 rows, Int64 cols, Double lambda, Double k)
        {
            WeibullDistribution ran = new WeibullDistribution();
            int n = (int)rows;
            int l = (int)cols;
            ran.Lambda = lambda;
            ran.Alpha = k;
            return Matrix.Zeros(n, l).ForEach(z => ran.NextDouble());
        }
    }
}
