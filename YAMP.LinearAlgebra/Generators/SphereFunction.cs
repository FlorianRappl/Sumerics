using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
    [Kind("LinearAlgebra")]
    [Description("The sphere function generates the x-, y-, and z-coordinates of a unit sphere for use with surf and mesh.")]
    sealed class SphereFunction : YFunction
    {
        [Description("Draws a surf plot of an 20-by-20 sphere in the current figure.")]
        [Example("[X, Y, Z] = sphere()", "Returns the three matrices with x-, y-, and Z coordinates for a unit sphere. The matrices are saved in the variables X, Y, Z.")]
        public SphereResult Invoke()
        {
            return Invoke(20);
        }

        [Description("Draws a surf plot of an n-by-n sphere in the current figure.")]
        [Example("[X, Y, Z] = sphere(30)", "Returns the three matrices with x-, y-, and Z coordinates for a unit sphere with n = 30. The 31x31 matrices are saved in the variables X, Y, Z.")]
        public SphereResult Invoke(Int64 n)
        {
            int dim = (int)(n + 1);

            if (dim < 2)
                throw new Exception("The argument n has to be at least 1.");

            var X = new Matrix(dim, dim); // x = sin(phi) * cos(theta)
            var Y = new Matrix(dim, dim); // y = sin(phi) * sin(theta)
            var Z = new Matrix(dim, dim); // z = cos(phi)

            var stheta = Table(0.0, 2.0 * Math.PI, dim, Math.Sin);
            var ctheta = Table(0.0, 2.0 * Math.PI, dim, Math.Cos);
            var sphi = Table(0.0, Math.PI, dim, Math.Sin);
            var cphi = Table(0.0, Math.PI, dim, Math.Cos);

            for (var j = 0; j < dim; j++)
            {
                for (var i = 0; i < dim; i++)
                {
                    X[i, j] = sphi[j] * ctheta[i];
                    Y[i, j] = sphi[j] * stheta[i];
                    Z[i, j] = cphi[j];
                }
            }

            return new SphereResult(X, Y, Z);
        }

        public class SphereResult
        {
            Matrix x;
            Matrix y;
            Matrix z;

            public SphereResult(Matrix x, Matrix y, Matrix z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            [Description("Gets the matrix with X values of the sphere.")]
            public Matrix X
            {
                get { return x; }
            }

            [Description("Gets the matrix with Y values of the sphere.")]
            public Matrix Y
            {
                get { return y; }
            }

            [Description("Gets the matrix with Z values of the sphere.")]
            public Matrix Z
            {
                get { return z; }
            }
        }

        double[] Table(double s, double e, int n, Func<double, double> f)
        {
            var a = new double[n];
            var c = s;
            var d = (e - s) / (n - 1);

            for (var i = 0; i < n; i++)
            {
                a[i] = f(c);
                c += d;
            }

            return a;
        }
    }
}
