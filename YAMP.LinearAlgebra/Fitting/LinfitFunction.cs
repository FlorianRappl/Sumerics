using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
    [Kind("LinearAlgebra")]
    [Description("Linear fitting by finding coefficients for a linear combination of functions. Curve fitting is the process of constructing a curve, or mathematical function, that has the best fit to a series of data points, possibly subject to constraints. Curve fitting can involve either interpolation, where an exact fit to the data is required, or smoothing, in which a smooth function is constructed that approximately fits the data.")]
    [Link("http://en.wikipedia.org/wiki/Curve_fitting")]
    sealed class LinfitFunction : YFunction
    {
        [Description("Linfit finds the coefficients of a linear combination of n functions, f_j(x(i)) to y(i), in a least squares sense. The result p is a row vector of length n containing the coefficients, i.e. p(1) * f_1 + p(2) * f_2 + p(3) * f_3 ... + p(n) * f_n.")]
        [Example("x=(-2.5:0.1:2.5); linfit(x, erf(x), [x, x.^3, tanh(x)])", "fits the error function with the function p(1) * x + p(2) * x.^3 + p(3) * tan(x). The result is p = [-0.149151; 0.006249; 1.291217]")]
        public Matrix Invoke(Matrix x, Matrix y, Matrix f)
        {
            if (x.Length != y.Length)
                throw new Exception("The vectors x and y have to have the same length.");

            if (x.Length != f.Rows)
                throw new Exception("The length of the vector x has to be equal to the number of rows of f.");

            var m = f.Columns;

            if (m < 2)
                throw new Exception("At least 2 columns in f are required...");

            var M = f;
            var b = new Matrix(x.Length, 1);

            for (var j = 0; j < M.Rows; j++)
                b[j, 0] = y[j];

            var qr = QRDecomposition.Create(M);
            return qr.Solve(b);
        }
    }
}
