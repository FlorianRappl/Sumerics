namespace Sumerics
{
    using System;
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

        public static Object Create(this IComponents container, Type type)
        {
            var ctors = type.GetConstructors();

            for (var i = 0; i < ctors.Length; i++)
            {
                var ctor = ctors[i];
                var parameters = ctor.GetParameters();
                var arguments = new Object[parameters.Length];

                for (var j = 0; j < arguments.Length; j++)
                {
                    arguments[j] = container.Get(parameters[j].ParameterType);
                }

                var result = ctor.Invoke(arguments);

                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public static T Create<T>(this IComponents container)
            where T : class
        {
            return container.Create(typeof(T)) as T;
        }
    }
}
