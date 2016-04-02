namespace Sumerics
{
    using Microsoft.Practices.Unity;
    using System;
    using System.Collections.Generic;

    public class Services : IServices
    {
        readonly UnityContainer _container;

        public Services()
        {
            _container = new UnityContainer();
        }

        public void Register<T>(T instance)
        {
            _container.RegisterInstance(instance);
        }

        public void Register<TService, TImplementation>()
            where TImplementation : TService
        {
            _container.RegisterType<TService, TImplementation>();
        }

        public Object Get(Type type)
        {
            if (!_container.IsRegistered(type))
            {
                var interfaces = type.GetInterfaces();

                if (interfaces.Length == 1 && _container.IsRegistered(interfaces[0]))
                {
                    var instance = _container.Resolve(interfaces[0]);
                    _container.RegisterInstance(type, instance);
                    return instance;
                }
            }

            return _container.Resolve(type);
        }

        public IEnumerable<Object> All(Type type)
        {
            return _container.ResolveAll(type);
        }
    }
}
