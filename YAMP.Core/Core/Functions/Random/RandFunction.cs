using System;
using YAMP.Attributes;
using YAMP.Numerics;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("Generates a matrix with uniformly distributed random values between 0 and 1.")]
    [Kind(KindAttribute.FunctionKind.Random)]
    [Link("http://en.wikipedia.org/wiki/Uniform_distribution_(continuous)")]
    sealed class RandFunction : YFunction
	{
		[Description("Generates one uniformly dist. random value between 0 and 1.")]
		public Double Invoke()
		{
            ContinuousUniformDistribution ran = new ContinuousUniformDistribution();
            return ran.NextDouble();
		}

		[Description("Generates a n-by-n matrix with uniformly dist. random values between 0 and 1.")]
		[Example("rand(5)", "Generates a 5x5 matrix with an uni. dist. rand. value in each cell.")]
        public Matrix Invoke(Int64 n)
        {
            ContinuousUniformDistribution ran = new ContinuousUniformDistribution();
            int k = (int)n;

			if (n < 1)
				n = 1;

            return Matrix.Zeros(k, k).ForEach(z => ran.NextDouble());
		}

		[Description("Generates a n-by-m matrix with uniformly dist. random values between 0 and 1.")]
		[Example("rand(5, 2)", "Generates a 5x2 matrix with an uni. dist. rand. value in each cell.")]
        public Matrix Invoke(Int64 rows, Int64 cols)
        {
            ContinuousUniformDistribution ran = new ContinuousUniformDistribution();
            var k = (int)rows;
            var l = (int)cols;
            return Matrix.Zeros(k, l).ForEach(z => ran.NextDouble());
		}
	}
}