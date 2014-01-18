using System;
using System.Collections.Generic;
using YAMP.Core;
using YAMP.Tables;

namespace YAMP.Core
{
    /// <summary>
    /// Represents a scope.
    /// </summary>
    class Scope
    {
        Scope _parent;

        readonly List<KeyValuePair<String, Dynamic>> _variables;

        public Scope()
        {
            _parent = null;
            _variables = new List<KeyValuePair<String, Dynamic>>();
        }

        public Scope Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public Scope ScopeOf(String name)
        {
            for (int i = 0; i < _variables.Count; i++)
            {
                if (_variables[i].Key == name)
                    return this;
            }

            if (_parent != null)
                return _parent.ScopeOf(name);

            return null;
        }

        public void Set(String name, Dynamic value)
        {
            for (int i = 0; i < _variables.Count; i++)
            {
                if (_variables[i].Key == name)
                {
                    if (value == null)
                        _variables.RemoveAt(i);
                    else
                        _variables[i] = new KeyValuePair<String, Dynamic>(name, value);

                    return;
                }
            }

            _variables.Add(new KeyValuePair<String, Dynamic>(name, value));
        }

        public Dynamic Get(String name)
        {
            for (int i = 0; i < _variables.Count; i++)
            {
                if (_variables[i].Key == name)
                    return _variables[i].Value;
            }

            if (_parent != null)
                return _parent.Get(name);

            return null;
        }

        public override String ToString()
        {
            var str = new String[_variables.Count];
            var k = 0;

            foreach (var pair in _variables)
                str[k++] = pair.Key + " = " + pair.Value;

            return String.Join(", ", str);
        }
    }
}
