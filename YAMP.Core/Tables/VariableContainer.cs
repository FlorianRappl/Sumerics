using System;
using System.Collections;
using System.Collections.Generic;
using YAMP.Core;

namespace YAMP.Tables
{
    sealed class VariableContainer : Container, IEnumerable<KeyValuePair<String, Dynamic>>
    {
        #region Members

        Dictionary<String, Dynamic> _table;

        #endregion

        #region ctor

        public VariableContainer()
        {
            _table = new Dictionary<String, Dynamic>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the variable with the specified name.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <returns>The value of the variable.</returns>
        public Dynamic this[String name]
        {
            get
            {
                if (_table.ContainsKey(name))
                    return _table[name];

                return null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a variable to the container.
        /// Already existing variables with the same name will be removed.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The value of the container.</param>
        public void Add(String name, Dynamic value)
        {
            if (_table.ContainsKey(name))
            {
                _table[name] = value;
                RaiseChanged(name, ChangeState.Changed);
            }
            else
            {
                _table.Add(name, value);
                RaiseChanged(name, ChangeState.Added);
            }
        }

        /// <summary>
        /// Removes all variables.
        /// </summary>
        public void Clear()
        {
            foreach (var item in _table)
                RaiseChanged(item.Key, ChangeState.Removed);

            _table.Clear();
        }

        /// <summary>
        /// Removes a variable with the given name.
        /// </summary>
        /// <param name="name">The name of the variable to remove.</param>
        public void Remove(String name)
        {
            if (_table.ContainsKey(name))
            {
                _table.Remove(name);
                RaiseChanged(name, ChangeState.Removed);
            }
        }

        /// <summary>
        /// Removes all variables that are connected to the specified function.
        /// </summary>
        /// <param name="function">The kind of variable to remove.</param>
        public void RemoveFrom(IFunction function)
        {
            var dz = new Queue<string>();

            foreach (var item in _table)
            {
                if(item.Value.Value == function)
                    dz.Enqueue(item.Key);
            }

            while (dz.Count != 0)
                Remove(dz.Dequeue());
        }

        #endregion

        #region IEnumerable implementation

        public IEnumerator<KeyValuePair<String, Dynamic>> GetEnumerator()
        {
            foreach (var variable in _table)
                yield return variable;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
