namespace Sumerics
{
    using System;
    using System.Collections.Generic;

    public static class GeneralExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> callback)
        {
            foreach (var element in collection)
            {
                callback(element);
            }
        }
    }
}
