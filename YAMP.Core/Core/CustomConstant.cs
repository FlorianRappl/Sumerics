using System;

namespace YAMP.Core
{
    /// <summary>
    /// Just to create a simple constant without much problems
    /// </summary>
    public sealed class CustomConstant : YConstant
    {
        #region Members

        object _value;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new instance of a custom constant.
        /// </summary>
        /// <param name="name">The name of the constant.</param>
        /// <param name="value">The value of the constant.</param>
        public CustomConstant(string name, object value)
            : base(name)
        {
            _value = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the value of the custom constant.
        /// </summary>
        public override object Value
        {
            get { return _value; }
        }

        #endregion
    }
}
