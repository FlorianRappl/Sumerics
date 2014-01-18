using System;
using System.Collections.Generic;

namespace YAMP.Core
{
    /// <summary>
    /// Represents a YAMP² plugin.
    /// </summary>
    public abstract class YPlugin
    {
        #region Members

        List<IFunction> _functions;
        List<IConstant> _constants;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new YAMP plugin package initializer.
        /// </summary>
        public YPlugin()
        {
            _constants = new List<IConstant>();
            _functions = new List<IFunction>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// Gets the author of the plugin.
        /// </summary>
        public abstract string Author
        {
            get;
        }

        /// <summary>
        /// Gets the version of the plugin.
        /// </summary>
        public abstract string Version
        {
            get;
        }

        /// <summary>
        /// Gets the list of functions in this plugin.
        /// </summary>
        internal List<IFunction> Functions
        {
            get { return _functions; }
        }

        /// <summary>
        /// Gets the list of constants in this plugin.
        /// </summary>
        internal List<IConstant> Constants
        {
            get { return _constants; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Includes a function in the plugin.
        /// </summary>
        /// <param name="function">The function to include.</param>
        protected void IncludeFunction(IFunction function)
        {
            CheckName(function.Name);
            _functions.Add(function);
        }

        /// <summary>
        /// Includes a constant in the plugin.
        /// </summary>
        /// <param name="constant">The constant to include.</param>
        protected void IncludeConstant(IConstant constant)
        {
            CheckName(constant.Name);
            _constants.Add(constant);
        }

        void CheckName(String name)
        {
            if (!YAMP.Parser.Chars.IsIdentifier(name))
                throw new Exception("The given function name " + name + " is not valid.");
        }

        #endregion
    }
}
