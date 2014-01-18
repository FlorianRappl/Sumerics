using System;
using System.Collections.Generic;
using YAMP.Tables;
using YAMP.Types;

namespace YAMP.Core
{
    /// <summary>
    /// Information for the runtime.
    /// </summary>
    public sealed class RunContext
    {
        #region Members

        Workspace _core;
        VariableContainer _variables;
        FunctionContainer _libFunctions;
        ConstantContainer _libConstants;
        WeakReference _return;
        Stack<Boolean> _break;
        Stack<Scope> _scopes;

        #endregion

        #region ctor

        internal RunContext(Workspace core)
        {
            _core = core;
            _variables = core.Context.Variables;
            _libFunctions = core.Extensions.Functions;
            _libConstants = core.Extensions.Constants;
            _break = new Stack<Boolean>();
            _scopes = new Stack<Scope>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the return value.
        /// </summary>
        internal Dynamic ReturnValue
        {
            get { return _return != null && _return.IsAlive ? _return.Target as Dynamic : null; }
        }

        /// <summary>
        /// Gets the current scope or null.
        /// </summary>
        internal Scope Current
        {
            get { return _scopes.Count != 0 ? _scopes.Peek() : null; }
        }

        /// <summary>
        /// Gets if the current evaluation should be stopped.
        /// </summary>
        internal Boolean ShouldStop
        {
            get { return (_break.Count != 0 && _break.Peek()) || (_return != null); }
        }

        /// <summary>
        /// Gets a collection of constants.
        /// </summary>
        internal IEnumerable<IConstant> Constants
        {
            get
            {
                foreach (var constant in _core.Context.Constants)
                    yield return constant;

                foreach (var constant in _core.Extensions.Constants)
                    yield return constant;
            }
        }

        /// <summary>
        /// Gets a collection of functions.
        /// </summary>
        internal IEnumerable<IFunction> Functions
        {
            get
            {
                foreach (var function in _core.Context.Functions)
                    yield return function;

                foreach (var function in _core.Extensions.Functions)
                    yield return function;
            }
        }

        /// <summary>
        /// Gets a collection of variables.
        /// </summary>
        internal IEnumerable<KeyValuePair<String, Dynamic>> Variables
        {
            get
            {
                foreach (var variable in _core.Context.Variables)
                    yield return variable;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the plugin with the specified name if any.
        /// </summary>
        /// <param name="name">The name of the plugin.</param>
        /// <returns>Returns the plugin or null.</returns>
        public YPlugin GetPlugin(String name)
        {
            return _core.Extensions.Find(name);
        }

        /// <summary>
        /// Gets the specified constant or null.
        /// </summary>
        /// <param name="name">The name of the constant.</param>
        /// <returns>The constant instance or null.</returns>
        public IConstant GetConstant(String name)
        {
            return _core.Context.Constants[name] ?? _libConstants[name];
        }

        /// <summary>
        /// Gets the function with the specified name.
        /// </summary>
        /// <param name="name">The name of the function.</param>
        /// <returns>The function instance or null.</returns>
        public IFunction GetFunction(String name)
        {
            return _core.Context.Functions[name] ?? _libFunctions[name];
        }

        /// <summary>
        /// Gets the value of the variable with the specified name.
        /// </summary>
        /// <param name="name">The variable's name.</param>
        /// <returns>The value or null.</returns>
        public Dynamic GetVariable(String name)
        {
            return Current.Get(name) ?? _variables[name];
        }

        /// <summary>
        /// Sets the value of the variable with the given name.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The value of the variable.</param>
        public void SetVariable(String name, Dynamic value)
        {
            if (Current.Get(name) != null)
                Current.ScopeOf(name).Set(name, value);
            else if (value == null)
                _variables.Remove(name);
            else
                _variables.Add(name, value);
        }

        /// <summary>
        /// Sets the standard answer.
        /// </summary>
        /// <param name="value">The value of the answer.</param>
        internal void SetAnswer(Dynamic value)
        {
            if(_core.Runtime.Configuration.UseDefaultVariable)
                _variables.Add(_core.Runtime.Configuration.DefaultVariableName, value);
        }

        /// <summary>
        /// Sets a custom function with the given name in
        /// the loose functions container.
        /// </summary>
        /// <param name="name">The name of the loose function.</param>
        /// <returns>The existing or new custom function.</returns>
        internal CustomFunction SetFunction(String name)
        {
            var f = _core.Context.Functions[name] as CustomFunction;

            if(f != null)
                return f;

            f = new CustomFunction(name);
            _core.Context.Functions.Add(f);
            return f;
        }

        #endregion

        #region Scopes, Breaks and Returns

        internal Dynamic ClearReturnMarker()
        {
            var result = ReturnValue;
            _return = null;
            return result;
        }

        internal Scope ReleaseBreakableScope()
        {
            _break.Pop();
            return ReleaseScope();
        }

        internal void CreateBreakableScope(Scope scope)
        {
            _break.Push(false);
            CreateScope(scope);
        }

        internal Scope ReleaseScope()
        {
            return _scopes.Pop();
        }

        internal void CreateScope(Scope scope)
        {
            if(_scopes.Count != 0 && scope.Parent == null)
                scope.Parent = _scopes.Peek();

            _scopes.Push(scope);
        }

        internal void CreateTopScope(Scope scope)
        {
            _scopes.Push(scope);
        }

        /// <summary>
        /// Breaks the current execution.
        /// </summary>
        internal void Break()
        {
            if (_break.Count != 0)
            {
                _break.Pop();
                _break.Push(true);
            }
        }

        /// <summary>
        /// Returns the given object.
        /// </summary>
        /// <param name="value">The object to return.</param>
        internal void Return(Dynamic value)
        {
            _return = new WeakReference(value);
        }

        #endregion
    }
}
