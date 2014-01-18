using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("Outputs the dimensions (size) of the given object.")]
    [Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class SizeFunction : YFunction
    {
        [Description("Returns a row vector containing the number of rows (1, 1) and the number of columns (1, 2).")]
        [Example("size([1, 2, 3, 4, 5])", "Results in a vector with the elements 1 and 5, since we have 5 columns and 1 row.")]
        [Example("size(rand(2))", "Results in a vector with the elements 2 and 2, since we have 2 columns and 2 rows.")]
        public Int64[] Invoke(Matrix M)
        {
            return new long[] { M.Rows, M.Columns };
        }
    }
}
