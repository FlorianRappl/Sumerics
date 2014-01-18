using System;
using System.Collections.Generic;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("Converts a octal number to a decimal number.")]
    [Kind(KindAttribute.FunctionKind.Conversion)]
    sealed class Oct2DecFunction : YFunction
    {
        [Description("The function ignores white spaces and converts the given octal input to the equivalent decimal number.")]
        [Example("oct2dec(\"1627\")", "Octal 1627 converts to decimal 919.")]
        public Int64 Invoke(String octstr)
        {
            var sum = 0L;

            for (var i = 0; i < octstr.Length; i++)
            {
                var chr = octstr[i];

                if (char.IsWhiteSpace(chr))
                    continue;

                sum = sum << 3;

                if (chr >= '0' && chr <= '7')
                    sum += (long)(chr - '0');
                else
                    throw new YException("oct2dec can only interpret octal strings.");
            }

            return sum;
        }
    }
}
