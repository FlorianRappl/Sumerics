namespace Sumerics
{
    using LightInject;
    using System;
    using System.Collections.Generic;

    public sealed class Components : IComponents
    {
        readonly ServiceContainer _block;

        public Components()
        {
            _block = new ServiceContainer();
            _block.RegisterInstance<IComponents>(this);
            _block.RegisterFallback((t, s) => true, request =>
            {
                var type = request.ServiceType;
                var ctors = type.GetConstructors();

                for (var i = 0; i < ctors.Length; i++)
                {
                    var ctor = ctors[i];
                    var parameters = ctor.GetParameters();
                    var arguments = new Object[parameters.Length];

                    for (var j = 0; j < arguments.Length; j++)
                    {
                        arguments[j] = _block.GetInstance(parameters[j].ParameterType);
                    }

                    var result = ctor.Invoke(arguments);

                    if (result != null)
                    {
                        return result;
                    }
                }

                return null;
            });
        }

        public void Register<T>(T instance)
        {
            _block.RegisterInstance(instance);
        }

        public void Register<TService, TImplementation>()
            where TImplementation : TService
        {
            _block.Register<TService, TImplementation>();
        }

        public Object Get(Type type)
        {
            return _block.GetInstance(type);
        }

        public IEnumerable<Object> All(Type type)
        {
            return _block.GetAllInstances(type);
        }

        public Object Create(Type type)
        {
            return _block.Create(type);
        }
    }
}
