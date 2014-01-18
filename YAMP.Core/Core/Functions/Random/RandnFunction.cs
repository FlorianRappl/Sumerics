using System;
using YAMP.Attributes;
using YAMP.Numerics;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("Generates a matrix with normal distributed random values. In probability theory, the normal (or Gaussian) distribution is a continuous probability distribution, defined on the entire real line, that has a bell-shaped probability density function, known as the Gaussian function or informally as the bell curve.")]
    [Kind(KindAttribute.FunctionKind.Random)]
    [Link("http://en.wikipedia.org/wiki/Normal_distribution")]
    sealed class RandnFunction : YFunction
	{	
		[Description("Generates one normally (gaussian) distributed random value around 0 with standard deviation 1.")]
		public Double Invoke()
		{
            NormalDistribution ran = new NormalDistribution();
            ran.Sigma = 1.0;
            ran.Mu = 0.0;
            return ran.NextDouble();
		}

		[Description("Generates a n-by-n matrix with normally (gaussian) distributed random value around 0 with standard deviation 1.")]
		[Example("randn(3)", "Gives a 3x3 matrix with normally dist. rand. values.")]
        public Matrix Invoke(Int64 n)
		{
            return Invoke(n, n);
		}

		[Description("Generates a m-by-n matrix with normally (gaussian) distributed random value around 0 with standard deviation 1.")]
		[Example("randn(3, 1)", "Gives a 3x1 matrix with normally dist. rand. values.")]
        public Matrix Invoke(Int64 rows, Int64 cols)
		{
            return Invoke(rows, cols, 0.0, 1.0);
		}

		[Description("Generates a m-by-n matrix with normally (gaussian) distributed random value around mu with standard deviation sigma.")]
		[Example("randn(3, 1, 10, 2.5)", "Gives a 3x1 matrix with normally dist. rand. values around 10 with standard deviation 2.5.")]
        public Matrix Invoke(Int64 rows, Int64 cols, Double mu, Double sigma)
		{
            NormalDistribution ran = new NormalDistribution();
            ran.Sigma = sigma;
            ran.Mu = mu;
			var k = (int)rows;
			var l = (int)cols;
            return Matrix.Zeros(k, l).ForEach(z => ran.NextDouble());
		}
	}
}

