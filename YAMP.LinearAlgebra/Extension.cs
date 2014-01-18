using System;
using System.Reflection;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.LinearAlgebra
{
    /// <summary>
    /// Creates a new instance of the linear algebra plugin.
    /// </summary>
    public class Extension : YPlugin
    {
        #region ctor

        /// <summary>
        /// Creates an instance of the LA extension.
        /// </summary>
        public Extension()
        {
            var empty = new Type[0];
            var allTypes = Assembly.GetExecutingAssembly().GetTypes();

            for (int i = 0; i < allTypes.Length; i++)
            {
                var type = allTypes[i];

                if (type.IsAbstract)
                    continue;
                else if (type.IsSubclassOf(typeof(YFunction)))
                {
                    var ctor = type.GetConstructor(empty);

                    if (ctor != null)
                    {
                        var function = ctor.Invoke(null) as YFunction;

                        if (function != null)
                            IncludeFunction(function);
                    }
                    
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the standard library.
        /// </summary>
        public override string Name
        {
            get { return "YAMP.LinearAlgebra"; }
        }

        /// <summary>
        /// Gets the name of the author.
        /// </summary>
        public override string Author
        {
            get 
            { 
                var attr = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                return attr.Length == 0 ? "Florian Rappl" : ((AssemblyCopyrightAttribute)attr[0]).Copyright;
            }
        }

        /// <summary>
        /// Gets the version of the STL.
        /// </summary>
        public override string Version
        {
            get
            {
                var attr = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyVersionAttribute), false);
                return attr.Length == 0 ? "1.0.0" : ((AssemblyVersionAttribute)attr[0]).Version;
            }
        }

        #endregion
    }
}
