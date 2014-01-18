using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
	[Description("Calculates the determinant of the given matrix.")]
	[Kind("LinearAlgebra")]
    [Link("http://en.wikipedia.org/wiki/Determinant")]
    sealed class DetFunction : YFunction
	{
		[Description("Uses the best algorithm to compute the determinant.")]
		[Example("det([1,3;-1,0])", "Computes the determinant of the matrix [1,3;-1,0]; returns 3.")]
		public Complex Invoke(Matrix M)
		{
			return M.Det();
		}
	}
}

