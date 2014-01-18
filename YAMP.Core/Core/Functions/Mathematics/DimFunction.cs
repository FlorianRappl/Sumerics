using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("Outputs the dimension of the given object.")]
    [Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class DimFunction : YFunction
	{
		[Description("Returns an integer containing the dimension of the given matrix.")]
		[Example("dim([1, 2, 3, 4, 5])", "Results in the value 5.")]
		public Int64 Invoke(Matrix M)
		{
            return Math.Max(M.Columns, M.Rows);
		}

        [Description("Returns an integer containing the length of the given string.")]
        [Example("dim(\"hello\")", "Results in the value 5.")]
        public Int64 Invoke(String s)
        {
            return s.Length;
        }
	}
}

