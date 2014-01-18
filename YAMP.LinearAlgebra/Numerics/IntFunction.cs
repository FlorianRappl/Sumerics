using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
    [Kind(KindAttribute.FunctionKind.Mathematics)]
    [Description("Integrates a given vector numerically by summing up all entries and returns the antiderivative vector.")]
    [Link("http://en.wikipedia.org/wiki/Numerical_integration")]
    sealed class IntFunction : YFunction
	{
		[Description("Integrates a given vector numerically by summing up all entries and returns the antiderivative vector.")]
        [Example("int([1,2,3,2;2,1,0,-1])", "Integrates the function values given in the matrix and returns the antiderivative matrix [0,1,3,6,8;0,2,3,3,2].")]
        public Matrix Invoke(Matrix M)
        {
            var adm = new Matrix(M.Rows, M.Columns + 1);

            for (int i = 0; i < M.Rows; i++)
            {
                adm[i, 0] = 0;

                for (int t = 0; t < M.Columns; t++)
                    adm[i, t + 1] = adm[i, t].Re + M[i, t].Re;
            }

            return adm;
        }
    }
}
