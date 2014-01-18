using System;
using System.Collections.Generic;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
    [Kind("LinearAlgebra")]
    [Description("The magic function randomly generates matrices that consist only of unique entries 1 <= k <= n^2, where n is the dimension of the matrix. The determinant of a magic matrix is always 0.")]
    sealed class MagicFunction : YFunction
    {
        [Description("Creates a magic nxn-matrix, that consists only of integer entries.")]
        [Example("magic(3)", "Creates a 3x3-matrix with unique shuffled entries ranging from 1 to 9.")]
        public Matrix Invoke(Int64 n)
        {
            var r = new DiscreteUniformDistribution();
            var l = (int)(n * n);
            var M = new Matrix((int)n, (int)n);
            var numbers = new List<int>();
            r.Alpha = 0;

            for (int i = 0; i < l; i++)
                numbers.Add(i);

            for (int j = 0; j < n; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    r.Beta = numbers.Count - 1;
                    var index = r.Next();
                    index = Math.Max(Math.Min(0, index), numbers.Count - 1);
                    M[j, i] = numbers[index];
                    numbers.RemoveAt(index);
                }
            }

            return M;
        }
    }
}
