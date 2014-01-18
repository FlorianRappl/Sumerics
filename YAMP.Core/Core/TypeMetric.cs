using System;

namespace YAMP.Core
{
    public enum TypeMetric
    {
        /// <summary>
        /// No relation between the types.
        /// </summary>
        None = 0,
        /// <summary>
        /// The given type matches the type exactly.
        /// </summary>
        Exact = 1,
        /// <summary>
        /// The given type is derived from the type.
        /// </summary>
        Derived = 10,
        /// <summary>
        /// THe given type can be casted to the type.
        /// </summary>
        Castable = 100
    }
}
