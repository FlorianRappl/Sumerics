using System;
using System.Collections;
using System.Collections.Generic;
using YAMP.Core;
using YAMP.Tables.Maps;

namespace YAMP.Tables
{
    /// <summary>
    /// Represents a container full of functions.
    /// </summary>
    sealed class FunctionContainer : Container, IEnumerable<IFunction>
    {
        #region Members

        Dictionary<String, IFunction> _table;

        #endregion

        #region ctor

        public FunctionContainer()
        {
            _table = new Dictionary<string, IFunction>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the function with the specified name.
        /// </summary>
        /// <param name="name">The name of the function.</param>
        /// <returns>The function.</returns>
        public IFunction this[String name]
        {
            get
            {
                if(_table.ContainsKey(name))
                    return _table[name];

                return null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a function to the container.
        /// Already existing function with the same name will be
        /// replaced (at least temporarly).
        /// </summary>
        /// <param name="name">The name of the function.</param>
        /// <param name="value">The function's value.</param>
        public void Add(IFunction value)
        {
            if (value.Resolver == null)
                value.Resolver = new MethodMapping(value.Overloads);

            if (_table.ContainsKey(value.Name))
            {
                _table[value.Name] = value;
                RaiseChanged(value.Name, ChangeState.Changed);
            }
            else
            {
                _table.Add(value.Name, value);
                RaiseChanged(value.Name, ChangeState.Added);
            }
        }

        /// <summary>
        /// Removes all functions.
        /// </summary>
        public void Clear()
        {
            foreach (var item in _table)
                RaiseChanged(item.Key, ChangeState.Removed);

            _table.Clear();
        }

        /// <summary>
        /// Removes a function with the given name.
        /// </summary>
        /// <param name="name">The name of the function to remove.</param>
        public void Remove(IFunction value)
        {
            if (_table.ContainsKey(value.Name))
            {
                _table.Remove(value.Name);
                RaiseChanged(value.Name, ChangeState.Removed);
            }
        }

        #endregion

        #region IEnumerable implementation

        public IEnumerator<IFunction> GetEnumerator()
        {
            foreach (var key in _table)
                yield return key.Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
