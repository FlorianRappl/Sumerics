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
        }

        public void Register<T>(T instance)
        {
            _block.RegisterInstance(instance);
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
