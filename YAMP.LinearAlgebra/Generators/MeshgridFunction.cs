using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
    [Kind("LinearAlgebra")]
    [Description("Generate X and Y matrices for three-dimensional plots.")]
    sealed class MeshgridFunction : YFunction
    {
        [Description("The function call is the same as [X, Y] = meshgrid(x, x), i.e. the input value is seen as both, X and Y vector.")]
        public MeshgridResult Invoke(Matrix x)
        {
            return Invoke(x, x);
        }

        [Description("Transforms the domain specified by vectors x and y into arrays X and Y, which can be used to evaluate functions of two variables and three-dimensional mesh/surface plots. The rows of the output array X are copies of the vector x; columns of the output array Y are copies of the vector y.")]
        [Example("meshgrid(1:3, 10:14)", "Creates the X and Y matrices with X having the values 1 to 3 in each row, while Y has the values 10 to 14 in each column.")]
        public MeshgridResult Invoke(Matrix x, Matrix y)
        {
            var M = x.Length;
            var N = y.Length;
            var X = new Matrix(N, M);
            var Y = new Matrix(N, M);

            for (var i = 0; i < N; i++)
                for (var j = 0; j < M; j++)
                    X[i, j] = x[j];

            for (var i = 0; i < N; i++)
                for (var j = 0; j < M; j++)
                    Y[i, j] = y[i];

            return new MeshgridResult(X, Y);
        }

        public class MeshgridResult
        {
            Matrix x;
            Matrix y;

            public MeshgridResult(Matrix x, Matrix y)
            {
                this.x = x;
                this.y = y;
            }

            [Description("Gets the generated X value matrix.")]
            public Matrix X
            {
                get { return x; }
            }

            [Description("Gets the generated Y value matrix.")]
            public Matrix Y
            {
                get { return y; }
            }
        }
    }
}
