using System;
using System.Collections.Generic;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("Converts a vector given in spherical coordinates to a cartesian coordinates.")]
    [Kind(KindAttribute.FunctionKind.Conversion)]
    sealed class Sph2CartFunction : YFunction
    {
        [Description("Converts a set of values (r, theta, phi) to a column vector with 3 rows (x, y, z).")]
        [Example("sph2cart(4, pi/2, pi/4)", "Computes the cartesian coordinates of the given spherical coordinates r = 4, theta = pi / 2 and phi = pi /4.")]
        public Matrix Invoke(Double r, Double phi, Double theta)
        {
            var rt = r * Math.Sin(theta);
            var x = rt * Math.Cos(phi);
            var y = rt * Math.Sin(phi);
            var z = r * Math.Cos(theta);
            return new Matrix(new[] { x, y, z }, 3, 1);
        }

        [Description("Converts a matrix of values (r, theta, phi in the rows or columns) to a matrix of converted values.")]
        [Example("sph2cart([1, pi/2, pi/4; 1, pi/3, pi/4; 1, pi/4, pi/4; 1, pi/5, pi/4])", "Evaluates the 4x3 matrix, using the columns as vectors (a set of row vectors to be converted).")]
        public Matrix Invoke(Matrix M)
        {
            if (M.Columns != 3 && M.Rows != 3)
                throw new YException("(sph2cart) Either the number of columns has to be 3 or the number of rows.");

            var isTransposed = M.Rows != 3;

            if (isTransposed)
                M = M.Trans();

            var m = new Matrix(3, M.Columns);

            for (var i = 0; i < M.Columns; i++)
            {
                var r = M[0, i].Re;
                var theta = M[1, i].Re;
                var phi = M[2, i].Re;
                var rt = r * Math.Sin(theta);
                m[0, i] = rt * Math.Cos(phi);
                m[1, i] = rt * Math.Sin(phi);
                m[2, i] = r * Math.Cos(theta);
            }

            return isTransposed ? m.Trans() : m;
        }
    }
}
