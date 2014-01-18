using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Statistics
{
    [Description("Computes a histogram that shows the distribution of data values.")]
    [Kind(KindAttribute.FunctionKind.Statistic)]
    sealed class HistogramFunction : YFunction
    {
        [Description("Bins the elements in vector Y into 10 equally spaced containers and outputs the number of elements in each container in a row vector. If Y is an m-by-p matrix, histogram treats the columns of Y as vectors and outputs a 10-by-p matrix n. Each column of n contains the results for the corresponding column of Y. No elements of Y can be complex or of type integer.")]
        [Example("histogram(rand(100, 1))", "Places 100 uniformly generated random numbers into 10 bins with a spacing that should be approximately 0.1.")]
        public Matrix Invoke(Matrix Y)
        {
            return Invoke(Y, 10);
        }

        [Description("Here x is a vector, such that the distribution of Y among length(x) bins with centers specified by x. For example, if x is a 5-element vector, histogram distributes the elements of Y into five bins centered on the x-axis at the elements in x, none of which can be complex.")]
        [Example("histogram(rand(100, 1), [0.1, 0.5, 0.9])", "Places 100 uniformly generated random numbers into 3 bins that center around 0.1, 0.5 and 0.9.")]
        public Matrix Invoke(Matrix y, Double[] x)
        {
            if (y.IsVector)
                return Calculator.Histogram(y, x);

            var M = new Matrix();

            for (var i = 0; i < y.Columns; i++)
            {
                var N = Calculator.Histogram(y.GetColumnVector(i), x);

                for (var j = 0; j < N.Length; j++)
                    M[j, i] = N[j];
            }

            return M;
        }

        [Description("Bins the elements in vector Y into nbins equally spaced containers and outputs the number of elements as before.")]
        [Example("histogram(rand(100, 1), 20)", "Places 100 uniformly generated random numbers into 20 bins with a spacing that should be approximately 0.05.")]
        public Matrix Invoke(Matrix Y, Int64 nbins)
        {
            if (Y.IsVector)
                return Calculator.Histogram(Y, (int)nbins);

            var M = new Matrix();

            for (var i = 0; i < Y.Columns; i++)
            {
                var N = Calculator.Histogram(Y.GetColumnVector(i), (int)nbins);

                for (var j = 0; j < N.Length; j++)
                    M[j, i] = N[j];
            }

            return M;
        }
    }
}
