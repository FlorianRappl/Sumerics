namespace Sumerics
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public static class GeneralExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> callback)
        {
            foreach (var element in collection)
            {
                callback(element);
            }
        }

        public static Byte ToHexColor(this String str, Int32 start)
        {
            var sub = str.Substring(start, 2);
            return Byte.Parse(sub, NumberStyles.HexNumber);
        }

        public static Byte ToRgbColor(this String str)
        {
            var sub = Math.Min(Int32.Parse(str), 255);
            return Convert.ToByte(sub);
        }
    }
}
