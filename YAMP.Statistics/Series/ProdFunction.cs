using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Statistics
{
	[Description("Computes the product of a given vector or the sum for each column vector of a matrix.")]
    [Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class ProdFunction : YFunction
    {
        [Description("Evaluates the vector(s) and outputs the product(s) of the vector(s).")]
        [Example("prod([1,2,3,4,5,6,7,-1])", "Computes the product of the vector, which is -5040 in this case.")]
        [Example("prod([1,2;3,4;5,6;7,-1])", "Computes the product of the two vectors, which are 105 and -48 in this case.")]
        public Object Invoke(Matrix m)
        {
            if (m.IsVector)
                return GetVectorProduct(m);

            var M = new Matrix(1, m.Columns);

            for (var i = 0; i < m.Columns; i++)
                M[0, i] = GetVectorProduct(m.GetColumnVector(i));

            return M;
        }

        Complex GetVectorProduct(Matrix vec)
		{
			Complex prod = 1.0;
			
			for(var i = 0; i < vec.Length; i++)
				prod = prod * vec[i];
			
			return prod;
		}
	}
}

