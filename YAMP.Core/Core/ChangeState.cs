using System;

namespace YAMP.Core
{
    /// <summary>
    /// The various modification states.
    /// </summary>
    public enum ChangeState : ushort
    {
        /// <summary>
        /// The element has been added.
        /// </summary>
        Added,
        /// <summary>
        /// The element has been removed.
        /// </summary>
        Removed,
        /// <summary>
        /// The element has changed.
        /// </summary>
        Changed
    }
}
