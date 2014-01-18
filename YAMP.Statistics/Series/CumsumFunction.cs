using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Statistics
{
    [Description("Computes the cumulative sum of the given arguments.")]
    [Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class CumsumFunction : YFunction
    {
        [Description("Computes the cumulative sum of a vector or a list of vectors, i.e. a matrix.")]
        [Example("cumsum([1, 2, 3, 0, 3, 2])", "Returns the vector [1, 3, 6, 6, 9, 11], which is the cumulative sum of the given vector.")]
        public Matrix Invoke(Matrix m)
        {
            if (m.IsVector)
                return GetVectorSum(m);

            var M = new Matrix(m.Rows, m.Columns);

            for (var i = 0; i < m.Columns; i++)
                M.SetColumnVector(i, GetVectorSum(m.GetColumnVector(i)));

            return M;
        }

        Matrix GetVectorSum(Matrix vec)
        {
            Matrix m = new Matrix(vec.Rows, vec.Columns);
            Complex sum = 0.0;

            for (var i = 0; i < vec.Length; i++)
            {
                sum += vec[i];
                m[i] = sum;
            }

            return m;
        }
    }
}
