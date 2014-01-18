using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
    [Kind("LinearAlgebra")]
	[Description("Generates a matrix with only ones.")]
    sealed class OnesFunction : YFunction
    {
        [Description("Generates a 1x1 matrix.")]
        public Matrix Invoke()
        {
            return Matrix.Ones(1, 1);
        }

        [Description("Generates an n-dimensional matrix with only ones.")]
        [Example("ones(5)", "Returns a 5x5 matrix with 1 in each cell.")]
        public Matrix Invoke(Int64 n)
        {
            var k = (int)n;
            return Matrix.Ones(k, k);
        }

        [Description("Generates a n-by-m matrix with only ones.")]
        [Example("ones(5,2)", "Returns a 5x2 matrix with 1 in each cell.")]
        public Matrix Invoke(Int64 rows, Int64 cols)
        {
            var k = (int)rows;
            var l = (int)cols;
            return Matrix.Ones(k, l);
        }
    }
}
