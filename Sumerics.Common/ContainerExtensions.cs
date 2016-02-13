namespace Sumerics
{
    using System.Collections.Generic;
    using System.Linq;

    public static class ContainerExtensions
    {
        public static T Get<T>(this IComponents container)
            where T : class
        {
            return container.Get(typeof(T)) as T;
        }

        public static IEnumerable<T> All<T>(this IComponents container)
        {
            return container.All(typeof(T)).OfType<T>();
        }

        public static T Create<T>(this IComponents container)
            where T : class
        {
            return container.Create(typeof(T)) as T;
        }
    }
}
