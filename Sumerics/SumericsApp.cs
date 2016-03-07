namespace Sumerics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    sealed class SumericsApp : Services, IApplication
    {
        public SumericsApp()
        {
            Register<IApplication>(this);
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
                        instance.RegisterComponents(this);
                    }
                }
            }
        }

        public void Shutdown()
        {
            App.Current.Shutdown();
        }
    }
}
