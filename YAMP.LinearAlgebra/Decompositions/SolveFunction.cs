using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
    [Kind("LinearAlgebra")]
	[Description("Solves a system of linear equations by picking the best algorithm.")]
    sealed class SolveFunction : YFunction
	{
		[Description("Searches a solution vector x for the matrix M and the source vector phi.")]
		[Example("solve(rand(3), rand(3,1))", "Solves the equation M * x = b for a random 3x3 matrix M and a random vector phi.")]
        public Matrix Invoke(Matrix M, Matrix phi)
		{
			if (M.IsSymmetric)
			{
				var cg = new CGSolver(M);
				return cg.Solve(phi);
			}
			else if (M.Columns == M.Rows && M.Rows > 64) // Is there a way to "guess" a good number for this?
			{
				var gmres = new GMRESkSolver(M);
				gmres.Restart = 30;
				return gmres.Solve(phi);
			}

			return M.Inv() * phi;
		}
	}
}
