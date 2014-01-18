using System;
using YAMP.Attributes;
using YAMP.Numerics;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("Generates a matrix with uniformly distributed integer values.")]
    [Kind(KindAttribute.FunctionKind.Random)]
    [Link("http://en.wikipedia.org/wiki/Uniform_distribution_(discrete)")]
    sealed class RandiFunction : YFunction
	{
		[Description("Generates one uniformly dist. integer value between min and max.")]
		[Example("randi(0, 10)", "Gets a random integer between 0 and 10 (both inclusive).")]
        public Int64 Invoke(Int64 min, Int64 max)
        {
            DiscreteUniformDistribution ran = new DiscreteUniformDistribution();
            ran.Alpha = (int)min;
            ran.Beta = (int)max;
            return ran.Next();
		}

		[Description("Generates a n-by-n matrix with uniformly dist. integer values between min and max.")]
		[Example("randi(5, 0, 10)", "Gets a 5x5 matrix with random integers between 0 and 10 (both inclusive).")]
        public Matrix Invoke(Int64 n, Int64 min, Int64 max)
        {
            DiscreteUniformDistribution ran = new DiscreteUniformDistribution();
            ran.Alpha = (int)min;
            ran.Beta = (int)max;
            var k = (int)n;

			if (k < 1)
				k = 1;

            return Matrix.Zeros(k, k).ForEach(z => ran.Next());
		}

		[Description("Generates a m-by-n matrix with uniformly dist. integer values between min and max.")]
		[Example("randi(5, 2, 0, 10)", "Gets a 5x2 matrix with random integers between 0 and 10 (both inclusive).")]
        public Matrix Invoke(Int64 rows, Int64 cols, Int64 min, Int64 max)
        {
            DiscreteUniformDistribution ran = new DiscreteUniformDistribution();
            ran.Alpha = (int)min;
            ran.Beta = (int)max;
            var k = (int)rows;
            var l = (int)cols;
            return Matrix.Zeros(k, l).ForEach(z => ran.Next());
		}
	}
}

