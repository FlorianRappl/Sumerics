using System;
using System.Collections;
using System.Collections.Generic;
using YAMP.Core;

namespace YAMP.Tables
{
    sealed class ConstantContainer : Container, IEnumerable<IConstant>
    {
        #region Members

        List<IConstant> _table;

        #endregion

        #region ctor

        public ConstantContainer()
        {
            _table = new List<IConstant>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the constant with the specified name.
        /// </summary>
        /// <param name="name">The name of the constant.</param>
        /// <returns>The constant.</returns>
        public IConstant this[string name]
        {
            get
            {
                for (int i = 0; i < _table.Count; i++)
                {
                    if (_table[i].Name == name)
                        return _table[i];   
                }

                return null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a constant to the container.
        /// </summary>
        /// <param name="value">The constant's instance.</param>
        public void Add(IConstant value)
        {
            var state = ChangeState.Added;

            for (int i = 0; i < _table.Count; i++)
            {
                if (_table[i].Name == value.Name)
                {
                    state = ChangeState.Changed;
                    break;
                }
            }

            _table.Add(value);
            RaiseChanged(value.Name, state);
        }

        /// <summary>
        /// Removes all constant.
        /// </summary>
        public void Clear()
        {
            for (int i = _table.Count - 1; i >= 0; i--)
                RaiseChanged(_table[i].Name, ChangeState.Removed);

            _table.Clear();
        }

        /// <summary>
        /// Removes a constant with the given instance.
        /// </summary>
        /// <param name="value">The instance of the constant to remove.</param>
        public void Remove(IConstant value)
        {
            if (_table.Remove(value))
            {
                var state = ChangeState.Removed;

                for (int i = 0; i < _table.Count; i++)
                {
                    if (_table[i].Name == value.Name)
                    {
                        state = ChangeState.Changed;
                        break;
                    }
                }

                RaiseChanged(value.Name, state);
            }
        }

        #endregion

        #region IEnumerable implementation

        public IEnumerator<IConstant> GetEnumerator()
        {
            foreach (var key in _table)
                yield return key;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
