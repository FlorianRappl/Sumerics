using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
    [Kind(KindAttribute.FunctionKind.Mathematics)]
    [Description("Differentiates a given vector numerically by differences and returns the derivative vector.")]
    [Link("http://en.wikipedia.org/wiki/Derivative")]
    sealed class DiffFunction : YFunction
	{
		[Description("Differentiates a given vector numerically by differences and returns the derivative vector.")]
        [Example("diff([0,1,3,6,8;0,2,3,3,2])", "Differentiates the function values given in the matrix and returns the antiderivative matrix [1,2,3,2;2,1,0,-1].")]
        public Matrix Invoke(Matrix M)
        {
            var adm = new Matrix(M.Rows, M.Columns - 1);

            for (int i = 0; i < M.Rows; i++)
            {
                for (int t = 0; t < M.Columns - 1; t++)
                    adm[i, t] = M[i, t + 1].Re - M[i, t].Re;
            }

            return adm;
        }
    }
}
