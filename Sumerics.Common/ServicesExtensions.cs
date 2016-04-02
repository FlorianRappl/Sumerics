namespace Sumerics
{
    using System.Collections.Generic;
    using System.Linq;

    public static class ServicesExtensions
    {
        public static T Get<T>(this IServices container)
            where T : class
        {
            return container.Get(typeof(T)) as T;
        }

        public static IEnumerable<T> All<T>(this IServices container)
        {
            return container.All(typeof(T)).OfType<T>();
        }
    }
}
