using System;
using System.Collections;
using System.Collections.Generic;
using YAMP.Core;

namespace YAMP.Tables
{
    /// <summary>
    /// Represents the libraries.
    /// </summary>
    sealed class Libraries : Container, IEnumerable<YPlugin>
    {
        #region Members

        List<YPlugin> _libs;
        FunctionContainer _functions;
        ConstantContainer _constants;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new libraries package.
        /// </summary>
        public Libraries()
        {
            _libs = new List<YPlugin>();
            _constants = new ConstantContainer();
            _functions = new FunctionContainer();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of extensions.
        /// </summary>
        public int Count
        {
            get { return _libs.Count; }
        }

        /// <summary>
        /// Gets the stored constants.
        /// </summary>
        public ConstantContainer Constants
        {
            get { return _constants; }
        }

        /// <summary>
        /// Gets the stored functions.
        /// </summary>
        public FunctionContainer Functions
        {
            get { return _functions; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Finds the plugin with the given name.
        /// </summary>
        /// <param name="name">The name of the plugin.</param>
        /// <returns>The plugin or null.</returns>
        public YPlugin Find(string name)
        {
            for (int i = 0; i < _libs.Count; i++)
            {
                if (_libs[i].Name == name)
                    return _libs[i];
            }

            return null;
        }

        /// <summary>
        /// Adds a plugin to the libraries.
        /// </summary>
        /// <param name="lib">The plugin to add.</param>
        public void Add(YPlugin lib)
        {
            if (!_libs.Contains(lib))
            {
                _libs.Add(lib);

                for (int i = 0; i < lib.Functions.Count; i++)
                    _functions.Add(lib.Functions[i]);

                for (int i = 0; i < lib.Constants.Count; i++)
                    _constants.Add(lib.Constants[i]);

                RaiseChanged(lib.Name, ChangeState.Added);
            }
        }

        /// <summary>
        /// Removes a plugin from the libraries.
        /// </summary>
        /// <param name="lib">The plugin to remove.</param>
        public void Remove(YPlugin lib)
        {
            if (_libs.Remove(lib))
            {
                for (int i = 0; i < lib.Functions.Count; i++)
                { }

                for (int i = 0; i < lib.Constants.Count; i++)
                    _constants.Remove(lib.Constants[i]);

                RaiseChanged(lib.Name, ChangeState.Removed);
            }
        }

        #endregion

        #region IEnumerable implementation

        public IEnumerator<YPlugin> GetEnumerator()
        {
            foreach (var lib in _libs)
                yield return lib;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
