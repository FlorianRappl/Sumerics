using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
    [Kind("LinearAlgebra")]
    [Description("Polynomial curve fitting by finding coefficients for constructing a polynom with degree n. Curve fitting is the process of constructing a curve, or mathematical function, that has the best fit to a series of data points, possibly subject to constraints. Curve fitting can involve either interpolation, where an exact fit to the data is required, or smoothing, in which a smooth function is constructed that approximately fits the data.")]
    [Link("http://en.wikipedia.org/wiki/Curve_fitting")]
    sealed class PolyfitFunction : YFunction
    {
        [Description("Polyfit finds the coefficients of a polynomial p(x) of degree n that fits the data, p(x(i)) to y(i), in a least squares sense. The result p is a row vector of length n + 1 containing the polynomial coefficients in ascending powers, i.e. p(1) + p(2) * x + p(3) * x^2 ... + p(n + 1) * x^n.")]
        [Example("polyfit(0:0.1:2.5, erf(0:0.1:2.5), 6)", "Evaluates the polynom of degree 6 of the error function between 0 and 2.5. The result is are coefficients for a polynom like p(x) = 0.0084 * x^6 - 0.0983 * x^5 + 0.4217 * x^4 - 0.7435 * x^3 + 0.1471 * x^2 + 1.1064 * x + 0.0004.")]
        public Matrix Invoke(Matrix x, Matrix y, Int64 n)
        {
            if (x.Length != y.Length)
                throw new Exception("The vectors x and y have to have the same length.");

            var m = (int)n + 1;

            if (m < 2)
                throw new Exception("The argument n has at least to be 1.");

            var M = new Matrix(x.Length, m);
            var b = new Matrix(x.Length, 1);

            for (var j = 0; j < M.Rows; j++)
            {
                var el = Complex.One;
                var z = x[j];

                for (var i = 0; i < M.Columns; i++)
                {
                    M[j, i] = el;
                    el *= z;
                }

                b[j, 0] = y[j];
            }

            var qr = QRDecomposition.Create(M);
            return qr.Solve(b);
        }
    }
}
