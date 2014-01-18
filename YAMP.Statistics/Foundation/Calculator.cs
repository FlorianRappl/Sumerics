using System;
using System.Linq;

namespace YAMP.Statistics
{
    /// <summary>
    /// Capsulates an ensemble of internally (frequently) used math functions.
    /// </summary>
    static class Calculator
    {
        public static Double Average(Double[] M)
        {
            if (M.Length == 0)
                return 0.0;

            var q = 0.0;

            for (var i = 1; i <= M.Length; i++)
                q += M[i];

            return q / M.Length;
        }

        public static Matrix Average(Matrix M)
        {
            var scale = 1.0;
            var s = new Matrix(1, M.Columns);

            for (var i = 0; i < M.Rows; i++)
                for (int j = 0; j < M.Columns; j++)
                    s[0, j] += M[i, j];

            scale /= M.Rows;

            for (int j = 0; j < s.Columns; j++)
                s[0, j] *= scale;

            return s;
        }

        public static Matrix Histogram(Matrix v, double[] centers)
        {
            if (centers.Length == 0)
                throw new Exception("There has to be at least one center position.");

            var H = new Matrix(centers.Length, 1);
            var N = new int[centers.Length];
            var last = centers.Length - 1;

            for (var i = 0; i < v.Length; i++)
            {
                var y = v[i].Re;

                if (y < centers[0])
                    N[0]++;
                else if (y > centers[last])
                    N[last]++;
                else
                {
                    var min = double.MaxValue;
                    var index = 0;

                    for (var j = 0; j < centers.Length; j++)
                    {
                        var dist = Math.Abs(y - centers[j]);

                        if (dist < min)
                        {
                            index = j;
                            min = dist;
                        }
                    }

                    N[index]++;
                }
            }

            for (var i = 0; i < centers.Length; i++)
                H[i, 0] = N[i];

            return H;
        }

        public static Matrix Histogram(Matrix v, int nbins)
        {
            var min = double.MaxValue;
            var max = double.MinValue;

            for (var i = 0; i < v.Length; i++)
            {
                if (v[i].Re > max)
                    max = v[i].Re;

                if (v[i].Re < min)
                    min = v[i].Re;
            }

            var delta = (max - min) / nbins;
            var D = new double[nbins];

            for (var i = 0; i < nbins; i++)
                D[i] = delta * (i + 0.5) + min;

            return Histogram(v, D);
        }

        public static Double Mean(Double[] M)
        {
            if (M.Length == 0)
                return 0.0;

            var q = 1.0;

            for (var i = 1; i <= M.Length; i++)
                q *= M[i];

            return Math.Pow(q, 1.0 / M.Length);
        }

        public static Matrix Mean(Matrix M)
        {
            var s = new Matrix(1, M.Columns);

            for (var i = 0; i < M.Columns; i++)
                s[0, i] = 1.0;

            for (var i = 0; i < M.Rows; i++)
                for (int j = 0; j < M.Columns; j++)
                    s[0, j] *= M[i, j];

            for (int j = 0; j < s.Columns; j++)
                s[0, j] = Complex.Pow(s[0, j], 1.0 / M.Rows);

            return s;
        }

        public static Double HarmonicMean(Double[] M)
        {
            if (M.Length == 0)
                return 0.0;

            var q = 0.0;

            for (var i = 1; i <= M.Length; i++)
                q += (1.0 / M[i]);

            return M.Length / q;
        }

        public static Matrix HarmonicMean(Matrix M)
        {
            var s = new Matrix(1, M.Columns);

            for (var i = 0; i < M.Columns; i++)
                s[0, i] = 0.0;

            for (var i = 0; i < M.Rows; i++)
                for (int j = 0; j < M.Columns; j++)
                    s[0, j] += (1.0 / M[i, j]);

            for (int j = 0; j < s.Columns; j++)
                s[0, j] = (M.Rows / s[0, j]);

            return s;
        }

        public static Matrix Covariance(Matrix M)
        {
            if (M.Length == 0)
                return new Matrix();

            if (M.IsVector)
                return new Matrix(new double[] { Variance(M.ToRealArray()) });

            var avg = Average(M);
            double scale = 1.0;
            var s = new Matrix(M.Rows, M.Columns);

            for (int i = 0; i < M.Rows; i++)
                for (int j = 0; j < M.Columns; j++)
                    for (int k = 0; k < M.Columns; k++)
                        s[k, j] += (M[i, j] - avg[j]) * (M[i, k] - avg[k]);

            scale /= M.Rows;
            return s.ForEach(m => m * scale);
        }

        public static Matrix Correlation(Matrix M)
        {
            if (M.Length == 0)
                return new Matrix();

            var result = Covariance(M);

            for (int i = 0; i < M.Rows; i++)
            {
                var temp = 1 / Complex.Sqrt(result[i, i]);

                for (int j = 0; j < M.Columns; j++)
                {
                    result[i, j] *= temp;
                    result[j, i] *= temp;
                }
            }

            return result;
        }

        public static Double[] CrossCorrelation(Double[] M, Double[] N, int n)
        {
            var result = new Double[n + 1];
            var avgM = Average(M);
            var avgN = Average(N);
            var errM = Math.Sqrt(Variance(M));
            var errN = Math.Sqrt(Variance(N));
            var length = M.Length;

            for (int i = 0; i <= n; i++)
            {
                var scale = 1.0 / ((length - i) * errM * errN);

                for (int j = 0; j < length - i; j++)
                    result[i] += (M[j] - avgM) * (N[j + i] - avgN);

                result[i] *= scale;
            }

            return result;
        }

        public static Double Variance(Double[] M)
        {
            if (M.Length == 0)
                return 0.0;

            var variance = 0.0;
            var mean = 0.0;

            for (int i = 0; i < M.Length; i++)
                mean += M[i];

            mean /= M.Length;

            for (int i = 0; i < M.Length; i++)
                variance += (M[i] - mean) * (M[i] - mean);

            return variance / M.Length;

            /*
            var avg = (MatrixValue)YMath.Average(M);
            var scale = 1.0;
            var s = new MatrixValue(1, M.DimensionX);

            for (var i = 1; i <= M.DimensionY; i++)
                for (int j = 1; j <= M.DimensionX; j++)
                    s[1, j] += (M[i, j] - avg[j]).Square();

            scale /= M.DimensionY;

            for (var i = 1; i <= s.DimensionY; i++)
                for (int j = 1; j <= s.DimensionX; j++)
                    s[i, j] *= scale;

            return s;*/
        }

        public static Double Median(Double[] M)
        {
            if (M.Length == 0)
                return 0.0;
            
            if (M.Length == 1)
                return M[1];
            
            M = M.OrderBy(m => m).ToArray();
            int midPoint;
            var sum = 0.0;

            if (M.Length % 2 == 1)
            {
                midPoint = M.Length / 2;
                sum = M[midPoint + 1];
            }
            else
            {
                midPoint = (M.Length / 2);
                sum = M[midPoint] + M[midPoint + 1];
                sum /= 2.0;
            }

            return sum;
        }
    }
}
