using System;
using System.Reflection;
using YAMP.Attributes;
using YAMP.Core.Constants;
using YAMP.Core.Functions;
using YAMP.Types;

namespace YAMP.Core
{
    /// <summary>
    /// The standard library of YAMPv2.
    /// </summary>
    sealed class StandardLibrary : YPlugin
    {
        #region ctor

        /// <summary>
        /// Creates an instance of the STL.
        /// </summary>
        public StandardLibrary(
            bool withMatrix = true, 
            bool withConstants = true, 
            bool withTrigonometric = true,
            bool withMathematics = true,
            bool withSystem = true,
            bool withRandom = true,
            bool withLogic = true,
            bool withConversion = true)
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
                    var kind = type.GetCustomAttributes(typeof(KindAttribute), false);
                    var category = kind.Length == 1 ? ((KindAttribute)kind[0]).Kind : "Custom";

                    if (category == KindAttribute.FunctionKind.Trigonometric.ToString() && !withTrigonometric)
                        continue;
                    else if (category == KindAttribute.FunctionKind.System.ToString() && !withSystem)
                        continue;
                    else if (category == KindAttribute.FunctionKind.Mathematics.ToString() && !withMathematics)
                        continue;
                    else if (category == KindAttribute.FunctionKind.Random.ToString() && !withRandom)
                        continue;
                    else if (category == KindAttribute.FunctionKind.Conversion.ToString() && !withConversion)
                        continue;
                    else if (category == KindAttribute.FunctionKind.Logic.ToString() && !withLogic)
                        continue;

                    var ctor = type.GetConstructor(empty);

                    if (ctor != null)
                    {
                        var function = ctor.Invoke(null) as YFunction;

                        if (function != null)
                            IncludeFunction(function);
                    }
                    
                }
                else if (withConstants && type.IsSubclassOf(typeof(YConstant)))
                {
                    var ctor = type.GetConstructor(empty);

                    if (ctor != null)
                    {
                        var constant = ctor.Invoke(null) as YConstant;

                        if (constant != null)
                            IncludeConstant(constant);
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
            get { return "YAMP.Core"; }
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
                return attr.Length == 0 ? "2.0.0" : ((AssemblyVersionAttribute)attr[0]).Version;
            }
        }

        #endregion
    }
}
