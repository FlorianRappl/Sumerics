using System;
using System.Collections.Generic;
using YAMP.Attributes;

namespace YAMP.Core
{
    public sealed class CustomFunction : IFunction, IFunctionResolver
    {
        #region Members

        String _name;
        Int32 _minp;
        Int32 _maxp;
        String _description;
        String _link;
        String _category;
        List<IFunctionItem> _overloads;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new custom function.
        /// </summary>
        /// <param name="name">The name of the function</param>
        public CustomFunction(string name)
        {
            _name = name;
            _category = KindAttribute.FunctionKind.General.ToString();
            _description = NoDescriptionAttribute.Message;
            _link = String.Empty;
            _overloads = new List<IFunctionItem>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a new anonymous custom function.
        /// </summary>
        public static CustomFunction Anonymous
        {
            get { return new CustomFunction("(anonymous)"); }
        }

        /// <summary>
        /// Gets the name of the function.
        /// </summary>
        public String Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the minimum number of required parameters.
        /// </summary>
        public Int32 MinParameters
        {
            get { return _minp; }
        }

        /// <summary>
        /// Gets the maximum number of required parameters.
        /// </summary>
        public Int32 MaxParameters
        {
            get { return _maxp; }
        }

        /// <summary>
        /// Gets or sets the description of the function.
        /// </summary>
        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Gets or sets the hyperlink with more information.
        /// </summary>
        public String HyperReference
        {
            get { return _link; }
            set { _link = value; }
        }

        /// <summary>
        /// Gets or sets the category of the function.
        /// </summary>
        public String Category
        {
            get { return _category; }
            set { _category = value; }
        }

        /// <summary>
        /// Gets the appropriate function resolver.
        /// </summary>
        public IFunctionResolver Resolver
        {
            get { return this; }
            set { }
        }

        /// <summary>
        /// Gets an enumerator iterating over all contained overloads.
        /// </summary>
        public IEnumerable<IFunctionItem> Overloads
        {
            get 
            {
                for (int i = 0; i < _overloads.Count; i++)
                    yield return _overloads[i]; 
            }
        }

        #endregion

        #region Methods

        public void AddOverload(IFunctionItem f)
        {
            if (_overloads.Count == 0)
            {
                _minp = f.Parameters.Length;
                _maxp = f.Parameters.Length;
                _overloads.Add(f);
            }
            else if(!_overloads.Contains(f))
            {
                for (int i = 0; i < _overloads.Count; i++)
                {
                    if (_overloads[i].Parameters.Length == f.Parameters.Length)
                    {
                        var match = true;

                        for (int j = 0; j < f.Parameters.Length; j++)
                        {
                            if (_overloads[i].Parameters[j] != f.Parameters[j])
                            {
                                match = false;
                                break;
                            }
                        }

                        if (match)
                        {
                            _overloads.RemoveAt(i);
                            break;
                        }
                    }
                }

                _minp = Math.Min(f.Parameters.Length, _minp);
                _maxp = Math.Max(f.Parameters.Length, _maxp);
                _overloads.Add(f);
            }
        }

        public void RemoveOverload(IFunctionItem f)
        {
            var index = _overloads.IndexOf(f);

            if (index != -1)
            {
                if (_overloads.Count == 1)
                {
                    _minp = 0;
                    _maxp = 0;
                }
                else if (_minp == f.Parameters.Length)
                {
                    _minp = _maxp;

                    for (int i = 0; i < _overloads.Count; i++)
                    {
                        if (i == index)
                            continue;

                        _minp = Math.Min(_minp, _overloads[i].Parameters.Length);
                    }
                }
                else if (_maxp == f.Parameters.Length)
                {
                    _maxp = _minp;

                    for (int i = 0; i < _overloads.Count; i++)
                    {
                        if (i == index)
                            continue;

                        _minp = Math.Max(_minp, _overloads[i].Parameters.Length);
                    }
                }

                _overloads.RemoveAt(index);
            }
        }

        #endregion

        #region Resolver

        /// <summary>
        /// Resolves the appropriate function for the given parameters.
        /// </summary>
        /// <param name="parameters">The parameters of the function.</param>
        /// <returns>A reference to a function or null.</returns>
        public Func<Dynamic[], Dynamic> Resolve(Dynamic[] parameters)
        {
            if (parameters.Length < _minp || parameters.Length > _maxp)
                return null;

            for (int i = 0; i < _overloads.Count; i++)
            {
                if (_overloads[i].Parameters.Length == parameters.Length)
                {
                    var match = true;

                    for (int j = 0; j < parameters.Length; j++)
                    {
                        if (!parameters[j].Type.IsType(_overloads[i].Parameters[j]))
                        {
                            match = false;
                            break;
                        }
                    }

                    if(match)
                        return _overloads[i].Function;
                }
            }

            return null;
        }

        #endregion

        #region String representation

        public override string ToString()
        {
            return "Function: " + _name;
        }

        #endregion
    }
}
