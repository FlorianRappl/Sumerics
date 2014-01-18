using System;
using System.Collections.Generic;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("Converts a vector given in cartesian coordinates to a polar coordinates.")]
    [Kind(KindAttribute.FunctionKind.Conversion)]
    sealed class Cart2PolFunction : YFunction
    {
        [Description("Converts a set of values (x, y) to a column vector with 2 rows (r, phi).")]
        [Example("cart2pol(3, 2)", "Computes the polar coordinates of the given cartesian coordinates x = 3, y = 2.")]
        public Matrix Invoke(Double x, Double y)
        {
            var phi = Math.Atan2(y, x);
            var r = x == 0.0 ? y : (y == 0.0 ? x : x / Math.Cos(phi));
            return new Matrix(new[] { r, phi }, 2, 1);
        }

        [Description("Converts a matrix of values (x, y in the rows or columns) to a matrix of converted values.")]
        [Example("cart2pol([1, 2; 4, -2; 1, 0; -2, 2])", "Evaluates the 4x2 matrix, using the columns as vectors (a set of row vectors to be converted).")]
        public Matrix Invoke(Matrix M)
        {
            if (M.Columns != 2 && M.Rows != 2)
                throw new YException("(cart2pol) Either the number of columns has to be 2 or the number of rows.");

            var isTransposed = M.Rows != 2;

            if (isTransposed)
                M = M.Trans();

            var m = new Matrix(2, M.Columns);

            for (var i = 0; i < M.Columns; i++)
            {
                var x = M[0, i].Re;
                var y = M[1, i].Re;
                var phi = Math.Atan2(y, x);
                var r = x == 0.0 ? y : (y == 0.0 ? x : x / Math.Cos(phi));
                m[0, i] = r * Math.Cos(phi);
                m[1, i] = r * Math.Sin(phi);
            }

            return isTransposed ? m.Trans() : m;
        }
    }
}
