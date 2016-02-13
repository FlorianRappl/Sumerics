namespace Sumerics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    sealed class SumericsApp : IApplication
    {
        readonly Components _components;

        public SumericsApp()
        {
            _components = new Components();
            _components.Register<IApplication>(this);
        }

        public void RegisterAssemblies()
        {
            var current = Assembly.GetExecutingAssembly();
            var assemblies = current.GetReferencedAssemblies();
            RegisterAssembly(current);
            RegisterAssemblies(assemblies);
        }

        public void RegisterAssemblies(IEnumerable<AssemblyName> assemblies)
        {
            foreach (var assemblyName in assemblies)
            {
                var assembly = Assembly.Load(assemblyName);
                RegisterAssembly(assembly);
            }
        }

        public void RegisterAssembly(Assembly assembly)
        {
            var types = assembly.DefinedTypes;

            foreach (var type in types)
            {
                if (type.ImplementedInterfaces.Contains(typeof(IModule)))
                {
                    var instance = type.GetConstructor(Type.EmptyTypes).Invoke(null) as IModule;

                    if (instance != null)
                    {
                        instance.RegisterComponents(_components);
                    }
                }
            }
        }

        public void Shutdown()
        {
            App.Current.Shutdown();
        }

        public IConsole Console
        {
            get { return _components.Get<IConsole>(); }
        }

        public IVisualizer Visualizer
        {
            get { return _components.Get<IVisualizer>(); }
        }

        public IKernel Kernel
        {
            get { return _components.Get<IKernel>(); }
        }

        public ITabs Tabs
        {
            get { return _components.Get<ITabs>(); }
        }

        public IDialogManager Dialog
        {
            get { return _components.Get<IDialogManager>(); }
        }

        public ISettings Settings
        {
            get { return _components.Get<ISettings>(); }
        }

        public IComponents Components
        {
            get { return _components; }
        }
    }
}
