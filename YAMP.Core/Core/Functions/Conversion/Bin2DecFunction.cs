using System;
using System.Collections.Generic;
using YAMP.Attributes;

namespace YAMP.Core.Functions
{
    [Description("Converts a binary number to a decimal number.")]
    [Kind(KindAttribute.FunctionKind.Conversion)]
    sealed class Bin2DecFunction : YFunction
    {
        [Description("The function ignores white spaces and converts the given binary input to the equivalent decimal number.")]
        [Example("bin2dec(\"010111\")", "Binary 010111 converts to decimal 23.")]
        public Int64 Invoke(String binarystr)
        {
            var sum = 0L;

            for (var i = 0; i < binarystr.Length; i++)
            {
                var chr = binarystr[i];

                if (char.IsWhiteSpace(chr))
                    continue;

                sum = sum << 1;

                if (chr == '1')
                    sum++;
                else if (chr != '0')
                    throw new YException("bin2dec can only interpret binary strings.");
            }

            return sum;
        }
    }
}
