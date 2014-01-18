using System;
using System.Collections.Generic;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("Converts a vector given in cartesian coordinates to a spherical coordinates.")]
    [Kind(KindAttribute.FunctionKind.Conversion)]
    sealed class Cart2SphFunction : YFunction
    {
        [Description("Converts a set of values (x, y, z) to a column vector with 3 rows (r, phi, theta).")]
        [Example("cart2sph(3, 2, 1)", "Computes the spherical coordinates of the given cartesian coordinates x = 3, y = 2 and z = 1.")]
        public Matrix Invoke(Double x, Double y, Double z)
        {
            var r = Math.Sqrt(x * x + y * y + z * z);
            var phi = Math.Atan2(y, x);
            var theta = Math.Acos(z / r);
            return new Matrix(new[] { r, theta, phi }, 3, 1);
        }

        [Description("Converts a matrix of values (x, y, z in the rows or columns) to a matrix of converted values.")]
        [Example("cart2sph([1, 2, 3; 4, -2, 0; 1, 0, -1; -2, 2, 1])", "Evaluates the 4x3 matrix, using the columns as vectors (a set of row vectors to be converted).")]
        public Matrix Invoke(Matrix M)
        {
            if (M.Columns != 3 && M.Rows != 3)
                throw new YException("(cart2sph) Either the number of columns has to be 3 or the number of rows.");

            var isTransposed = M.Rows != 3;

            if (isTransposed)
                M = M.Trans();

            var m = new Matrix(3, M.Columns);

            for (var i = 0; i < M.Columns; i++)
            {
                var x = M[0, i].Re;
                var y = M[1, i].Re;
                var z = M[2, i].Re;
                var r = Math.Sqrt(x * x + y * y + z * z);
                var phi = Math.Atan2(y, x);
                var theta = Math.Acos(z / r);
                m[0, i] = r;
                m[1, i] = theta;
                m[2, i] = phi;
            }

            return isTransposed ? m.Trans() : m;
        }
    }
}
