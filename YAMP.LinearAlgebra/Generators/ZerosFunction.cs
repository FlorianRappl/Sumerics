using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
    [Kind("LinearAlgebra")]
	[Description("Generates a matrix with only zeros.")]
    sealed class ZerosFunction : YFunction
    {
        [Description("Generates a 1x1 matrix.")]
        public Matrix Invoke()
        {
            return new Matrix(1, 1);
        }

        [Description("Generates an n-dimensional matrix with only zeros.")]
        [Example("zeros(5)", "Returns a 5x5 matrix with 0 in each cell.")]
        public Matrix Invoke(Int64 dim)
        {
            var k = (int)dim;
            return new Matrix(k, k);
        }

        [Description("Generates a n-by-m matrix with only zeros.")]
        [Example("zeros(5,2)", "Returns a 5x2 matrix with 0 in each cell.")]
        public Matrix Invoke(Int64 rows, Int64 cols)
        {
            var k = (int)rows;
            var l = (int)cols;
            return new Matrix(k, l);
        }
    }
}
