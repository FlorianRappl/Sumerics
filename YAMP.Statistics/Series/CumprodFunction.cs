using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Statistics
{
    [Description("Computes the cumulative product of the given arguments.")]
    [Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class CumprodFunction : YFunction
    {
        [Description("Computes the cumulative product of a vector or a list of vectors, i.e. a matrix.")]
        [Example("cumprod([1, 2, 3, 0, 3, 2])", "Returns the vector [1, 2, 6, 0, 0, 0], which is the cumulative product of the given vector.")]
        public Matrix Invoke(Matrix m)
        {
            if (m.IsVector)
                return GetVectorProd(m);

            var M = new Matrix(m.Rows, m.Columns);

            for (var i = 0; i < m.Columns; i++)
                M.SetColumnVector(i, GetVectorProd(m.GetColumnVector(i)));

            return M;
        }

        Matrix GetVectorProd(Matrix vec)
        {
            Matrix m = new Matrix(vec.Columns, vec.Rows);
            Complex prod = 1.0;

            for (var i = 0; i < vec.Length; i++)
            {
                prod *= vec[i];
                m[i] = prod;
            }

            return m;
        }
    }
}
