using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
	[Kind("LinearAlgebra")]
	[Description("Creates a diagonal matrix that has the given numeric values on the diagonal.")]
    sealed class DiagFunction : YFunction
    {
        [Description("Creates a diagonal matrix with the values from the given matrix.")]
        [Example("diag(rand(5))", "Creates a matrix with dimension 25 x 25, containing random values on the diagonal.")]
        [Example("diag(rand(5, 1))", "Creates a matrix with dimension 5 x 5, containing random values on the diagonal.")]
        public Matrix Invoke(Matrix M)
        {
            var m = new Matrix(M.Length, M.Length);

            for (var i = 0; i < M.Length; i++)
                m[i, i] = M[i];

            return m;
        }

		[Description("Tries to create a diagonal matrix with the given arguments, which must be of numeric nature, i.e. scalars or matrices.")]
		[Example("diag(1, 1, 1, 1)", "Creates the unit matrix with dimension 4.")]
		[Example("diag(1, 1, [0, 1; 1, 0], 1, 1)", "Creates a matrix that is close to the unit matrix, except that one block has been rotated in the middle.")]
		public Matrix Invoke(object[] values)
		{
			var M = new Matrix();

			for (var i = 0; i < values.Length; i++)
			{
				var sy = M.Rows;
				var sx = M.Columns;

                if (values[i] is Double)
                {
                    var s = (Double)values[i];
                    M[sy, sx] = s;
                }
                else if (values[i] is Complex)
                {
                    var s = (Complex)values[i];
                    M[sy, sx] = s;
                }
                else if (values[i] is Int64)
                {
                    var s = (Int64)values[i];
                    M[sy, sx] = s;
                }
                else if (values[i] is Boolean)
                {
                    var s = (Boolean)values[i] ? 1 : 0;
                    M[sy, sx] = s;
                }
                else if (values[i] is Matrix)
                {
                    var n = (Matrix)values[i];

                    for (var l = 0; l < n.Columns; l++)
                    {
                        for (var k = 0; k < n.Rows; k++)
                            M[sy + k, sx + l] = n[k, l];
                    }
                }
                else
                    M[sy, sx] = 0;
			}

			return M;
		}
	}
}
