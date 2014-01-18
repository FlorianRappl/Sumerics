using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Statistics
{
	[Description("Computes the sum of a given vector or the sum for each column vector of a matrix.")]
    [Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class SumFunction : YFunction
    {
        [Description("Evaluates the vector(s) and outputs the sum(s) of the vector(s).")]
        [Example("sum([1,2,3,4,5,6,7,-1])", "Computes the sum of the vector, which is 27 in this case.")]
        [Example("sum([1,2;3,4;5,6;7,-1])", "Computes the sums of the two vectors, which are 16 and 11 in this case.")]
        public Object Invoke(Matrix m)
        {
            if (m.IsVector)
                return GetVectorSum(m);

            var M = new Matrix(1, m.Columns);

            for (var i = 0; i < m.Columns; i++)
                M[0, i] = GetVectorSum(m.GetColumnVector(i));

            return M;
        }

        Complex GetVectorSum(Matrix vec)
		{
			Complex sum = 0.0;
			
			for(var i = 0; i < vec.Length; i++)
				sum = sum + vec[i];
			
			return sum;
		}
	}
}

