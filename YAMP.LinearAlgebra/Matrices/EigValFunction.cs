using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
    [Kind("LinearAlgebra")]
	[Description("Computes the eigenvalues of a given matrix.")]
    sealed class EigValFunction : YFunction
	{
		[Description("Solves the eigenproblem of a matrix A and return a vector with all (+degenerate) eigenvalues.")]
		[Example("eigval([1,2,3;4,5,6;7,8,9])", "Returns a vector with the three eigenvalues 16.11684, -1.11684 and 0 of this 3x3 matrix.")]
        public Complex[] Invoke(Matrix M)
		{
            var ev = new Eigenvalues(M);
            var m = new Complex[ev.RealEigenvalues.Length];

			for (var i = 0; i < ev.RealEigenvalues.Length; i++)
				m[i] = new Complex(ev.RealEigenvalues[i - 1], ev.ImagEigenvalues[i - 1]);

			return m;
		}
	}
}
