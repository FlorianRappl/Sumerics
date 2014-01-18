using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP
{
	[Description("Sorts the columns of a given matrix or the complete vector.")]
    [Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class SortFunction : YFunction
	{
		[Description("Gives back the sorted columns of the passed matrix. The sorting is done from the lowest to the highest number, i.e. in ascending order.")]
		[Example("sort([1, 2, 0, 9, 5])", "Sorts the vector [1, 2, 0, 9, 5], resulting in [0, 1, 2, 5, 9].")]
		[Example("sort([1, 2, 0, 9, 5; 10, 8, 4, 6, 1])", "Sorts the matrix [1, 2, 0, 9, 5; 10, 8, 4, 6, 1], resulting in [0, 1, 2, 5, 9; 1, 4, 6, 8, 10].")]
		public Matrix Invoke(Matrix M)
		{
			if (M.IsVector)
				return M.Sort();

			var result = new Matrix();

			for(var i = 0; i < M.Rows; i++)
			{
				var vec = M.GetSubMatrix(i, i + 1, 0, M.Columns);
                vec = vec.Sort();
				result = result.AddRow(vec);
			}

			return result;
		}
	}
}
