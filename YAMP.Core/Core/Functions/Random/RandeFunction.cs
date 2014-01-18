using System;
using YAMP.Attributes;
using YAMP.Numerics;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("Generates a matrix with exponentially distributed random values. In probability theory and statistics, the exponential distribution (a.k.a. negative exponential distribution) is a family of continuous probability distributions. It describes the time between events in a Poisson process, i.e. a process in which events occur continuously and independently at a constant average rate. It is the continuous analogue of the geometric distribution.")]
    [Kind(KindAttribute.FunctionKind.Random)]
    [Link("http://en.wikipedia.org/wiki/Exponential_distribution")]
    sealed class RandeFunction : YFunction
	{
		[Description("Generates one exponentially distributed random value around 0 with lambda 1.")]
		[Example("rande()", "Outputs a scalar that has been generated with respect to the distribution.")]
        public Double Invoke()
		{
            ExponentialDistribution ran = new ExponentialDistribution();
            ran.Lambda = 1.0;
            return ran.NextDouble();
		}

		[Description("Generates a n-by-n matrix with exponentially distributed random values with lambda set to 1.")]
		[Example("rande(3)", "Gives a 3x3 matrix with normally dist. rand. values.")]
        public Matrix Invoke(Int64 n)
        {
			return Invoke(n, n);
		}

		[Description("Generates a m-by-n matrix with exponentially distributed random values with lambda set to 1.")]
		[Example("rande(3, 1)", "Gives a 3x1 matrix with normally dist. rand. values.")]
        public Matrix Invoke(Int64 rows, Int64 cols)
		{
            return Invoke(rows, cols, 1.0);
		}

		[Description("Generates a m-by-n matrix with exponentially distributed random values that have been generated with a specified lambda.")]
		[Example("rande(3, 1, 2.5)", "Gives a 3x1 matrix with exponentially distributed random values with lambda = 2.5.")]
        public Matrix Invoke(Int64 rows, Int64 cols, Double lambda)
        {
            ExponentialDistribution ran = new ExponentialDistribution();
            ran.Lambda = lambda;
            int n = (int)rows;
            int l = (int)cols;
			var m = new Matrix(n, l);
            return m.ForEach(z => ran.NextDouble());
		}
	}
}

