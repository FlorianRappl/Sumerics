using System;
using System.Collections.Generic;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("Converts a hexadecimal number to a decimal number.")]
    [Kind(KindAttribute.FunctionKind.Conversion)]
    sealed class Hex2DecFunction : YFunction
    {
        [Description("The function ignores white spaces and converts the given hexadecimal input to the equivalent decimal number.")]
        [Example("hex2dec(\"FF\")", "Hexadecimal FF converts to decimal 255.")]
        public Int64 Invoke(String hexstr)
        {
            var sum = 0L;

            for (var i = 0; i < hexstr.Length; i++)
            {
                var chr = hexstr[i];

                if (char.IsWhiteSpace(chr))
                    continue;

                sum = sum << 4;
                
                if (chr >= '0' && chr <= '9')
                    sum += (long)(chr - '0');
                else if (chr >= 'A' && chr <= 'F')
                    sum += ((long)(chr - 'A') + 10L);
                else if (chr >= 'a' && chr <= 'f')
                    sum += ((long)(chr - 'a') + 10L);
                else
                    throw new YException("hex2dec can only interpret hexadecimal strings.");
            }

            return sum;
        }
    }
}
