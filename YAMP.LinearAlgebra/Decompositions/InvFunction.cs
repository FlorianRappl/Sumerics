using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
	[Kind("LinearAlgebra")]
	[Description("Inverts the given matrix.")]
    sealed class InvFunction : YFunction
	{
		[Description("Finds the inverse of a given real number.")]
		[Example("inv(5)", "Inverts the number 5, resulting in 0.2.")]
		public Double Invoke(Double x)
		{
			return 1.0 / x;
		}

        [Description("Finds the inverse of a given complex number.")]
        [Example("inv(5i)", "Inverts the number 5, resulting in -0.2i.")]
        public Complex Invoke(Complex z)
        {
            return 1.0 / z;
        }

		[Description("Tries to find the inverse of the matrix, i.e. inv(A)=A^-1.")]
		[Example("inv([0,2;1,0])", "Inverts the matrix [0,2;1,0], resulting in [0,1;0.5,0].")]
        public Matrix Invoke(Matrix M)
		{
			return M.Inv();
		}
	}
}

