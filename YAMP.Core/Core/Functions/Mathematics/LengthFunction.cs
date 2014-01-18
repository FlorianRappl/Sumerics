using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("Outputs the length of the given object.")]
    [Kind(KindAttribute.FunctionKind.Mathematics)]
    sealed class LengthFunction : YFunction
	{
        [Description("Returns a scalar that is basically the number of rows times the number of columns.")]
        [Example("length([1, 2, 3, 4, 5; 6, 7, 8, 9, 10])", "Results in a scalar value of 10, since we have 5 columns and 2 rows.")]
        public Int64 Invoke(Matrix M)
        {
            return M.Length;
        }

        [Description("Returns the length of the string, which is the number of characters.")]
        [Example("length(\"hello\")", "This is evaluated to be 5, which is the number of characters in the given string.")]
        public Int64 Invoke(String str)
        {
            return str.Length;
        }
	}
}

