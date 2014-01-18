using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Statistics
{
	[Description("In probability theory and statistics, cross-correlation is a measure of how much two random variables are correlated at different offsets.")]
    [Kind(KindAttribute.FunctionKind.Statistic)]
    [Link("http://en.wikipedia.org/wiki/Cross-correlation")]
    sealed class XCorFunction : YFunction
	{
        [Description("This function returns a vector with cross-correlations for diferent offsets. All matrices are treated as vectors.")]
        [Example("xcor(3 + randn(100, 1), 10 + 2 * randn(100, 1))", "Gives the cross-correlation for two independent random variables of variance [1, 4] and different offsets.")]
        public Matrix Function(Matrix M, Matrix N)
		{
            if (M.Length != N.Length || M.Length <= 1)
                return new Matrix();

            int nOffset = (int)(10 * Math.Log10(M.Length));

            if(nOffset < 0)
                nOffset = 0;
            else if(nOffset >= M.Length)
                nOffset = M.Length - 1;

            return new Matrix(Calculator.CrossCorrelation(M.ToRealArray(), N.ToRealArray(), nOffset));
		}

        [Description("This function returns a vector with the defined cross-correlations for diferent offsets. All matrices are treated as vectors.")]
        [Example("xcor(3 + randn(100, 1), 10 + 2 * randn(100, 1), 4)", "Gives the first 4 cross-correlations for two independent random variables of variance [1, 4].")]
        public Matrix Function(Matrix M, Matrix N, Int64 nOffset)
        {
            if (M.Length != N.Length || M.Length <= 1)
                return new Matrix();

            if (nOffset < 0)
                nOffset = 0;
            else if (nOffset >= M.Length)
                nOffset = M.Length - 1;

            return new Matrix(Calculator.CrossCorrelation(M.ToRealArray(), N.ToRealArray(), (int)nOffset));
        }
    }
}