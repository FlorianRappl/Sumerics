using System;
using System.Collections.Generic;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("Converts a vector given in polar coordinates to a cartesian coordinates.")]
    [Kind(KindAttribute.FunctionKind.Conversion)]
    sealed class Pol2CartFunction : YFunction
    {
        [Description("Converts a set of values (r, phi) to a column vector with 2 rows (x, y).")]
        [Example("pol2cart(4, pi/2)", "Computes the cartesian coordinates of the given polar coordinates r = 4, phi = pi / 2.")]
        public Matrix Invoke(Double r, Double phi)
        {
            var x = r * Math.Cos(phi);
            var y = r * Math.Sin(phi);
            return new Matrix(new[] { x, y }, 2, 1);
        }

        [Description("Converts a matrix of values (r, phi, in the rows or columns) to a matrix of converted values.")]
        [Example("pol2cart([1, pi/2; 1, pi/3; 1, pi/4; 1, pi/5])", "Evaluates the 4x2 matrix, using the columns as vectors (a set of row vectors to be converted).")]
        public Matrix Invoke(Matrix M)
        {
            if (M.Columns != 2 && M.Rows != 2)
                throw new YException("(pol2cart) Either the number of columns has to be 2 or the number of rows.");

            var isTransposed = M.Rows != 2;

            if (isTransposed)
                M = M.Trans();

            var m = new Matrix(2, M.Columns);

            for (var i = 0; i < M.Columns; i++)
            {
                var r = M[0, i].Re;
                var phi = M[1, i].Re;
                m[0, i] = r * Math.Cos(phi);
                m[1, i] = r * Math.Sin(phi);
            }

            return isTransposed ? m.Trans() : m;
        }
    }
}
