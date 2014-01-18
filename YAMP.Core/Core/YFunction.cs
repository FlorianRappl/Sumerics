using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using YAMP.Attributes;

namespace YAMP.Core
{
    /// <summary>
    /// This class gives a base class for more sophisticated implementations of functions,
    /// where all methods called "Invoke" will be added automatically.
    /// </summary>
    public abstract class YFunction : IFunction
    {
        #region Members

        String _name;
        Int32 _min;
        Int32 _max;
        ReflectedFunctionItem[] _bindings;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new instance with the name given by convention.
        /// The convention is given by the rule that the name of the
        /// class is the lower case version of {Name}Function.
        /// </summary>
		public YFunction()
		{
            _name = GetType().Name.Replace("Function", String.Empty).ToLower();
            InitialBindings();
		}

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="name">The name for the function.</param>
        public YFunction(String name)
        {
            _name = name;
            InitialBindings();
        }

        #endregion

        #region Methods

        void InitialBindings()
        {
            _min = 0;
            _max = 0;
            var methods = GetType().GetMethods();
            var bindings = new List<ReflectedFunctionItem>();

            for (int i = 0; i < methods.Length; i++)
            {
                if (methods[i].Name == "Invoke")
                {
                    var parameters = methods[i].GetParameters();
                    var plength = parameters.Length;

                    if (bindings.Count == 0)
                    {
                        _min = plength;
                        _max = plength;
                    }
                    else
                    {
                        _min = Math.Min(_min, plength);
                        _max = Math.Max(_max, plength);
                    }

                    bindings.Add(new ReflectedFunctionItem(methods[i], this));
                }
            }

            _bindings = bindings.ToArray();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the function.
        /// </summary>
        public String Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the minimum number of parameters.
        /// </summary>
        public Int32 MinParameters
        {
            get { return _min; }
        }

        /// <summary>
        /// Gets the maximum number of parameters.
        /// </summary>
        public Int32 MaxParameters
        {
            get { return _max; }
        }

        /// <summary>
        /// Gets a description of the function, given by a
        /// [Description] attribute on the class.
        /// </summary>
        public String Description
        {
            get
            {
                var descriptions = GetType().GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (descriptions.Length == 0)
                    return NoDescriptionAttribute.Message;

                var lines = new String[descriptions.Length];

                for (int i = 0; i < descriptions.Length; i++)
                {
                    var da = (DescriptionAttribute)descriptions[i];
                    lines[i] = da.Description;
                }

                return String.Join(Environment.NewLine, lines);
            }
        }

        /// <summary>
        /// Gets an URL with information for the function, given by a
        /// [Link] attribute on the class.
        /// </summary>
        public String HyperReference
        {
            get
            {
                var objects = GetType().GetCustomAttributes(typeof(LinkAttribute), false);

                if (objects.Length == 0)
                    return String.Empty;

                return ((LinkAttribute)objects[0]).Url;
            }
        }

        /// <summary>
        /// Gets the category of the function.
        /// </summary>
        public String Category
        {
            get
            {
                var objects = GetType().GetCustomAttributes(typeof(KindAttribute), false);

                if (objects.Length == 0)
                    return KindAttribute.FunctionKind.General.ToString();

                return ((KindAttribute)objects[0]).Kind;
            }
        }

        /// <summary>
        /// Gets all the possible overloads (instances) for this function.
        /// </summary>
        public IEnumerable<IFunctionItem> Overloads
        {
            get { return _bindings; }
        }

        /// <summary>
        /// Gets or sets a function resolver.
        /// </summary>
        public IFunctionResolver Resolver
        {
            get;
            set;
        }

        #endregion

        #region Nested

        /// <summary>
        /// Represents the encapsulation of example data.
        /// </summary>
        class YExample : IExample
        {
            /// <summary>
            /// Gets or sets the code snippet.
            /// </summary>
            public String CodeSnippet
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the description.
            /// </summary>
            public String Description
            {
                get;
                set;
            }
        }

        /// <summary>
        /// Represents the encapsulation of function data.
        /// </summary>
        sealed internal class ReflectedFunctionItem : IFunctionItem
        {
            #region Members

            Func<Dynamic[], Dynamic> _function;
            Type[] _parameters;
            Object[] _examples;
            String[] _parameterNames;
            Type _return;

            #endregion

            #region ctor

            /// <summary>
            /// Creates a new YFunctionItem.
            /// </summary>
            /// <param name="function">The reflection data for the method.</param>
            /// <param name="target">The target / instance where the method is defined.</param>
            public ReflectedFunctionItem(MethodInfo function, Object target)
            {
                var param = function.GetParameters();
                _parameters = new Type[param.Length];
                _parameterNames = new String[_parameters.Length];
                _examples = function.GetCustomAttributes(typeof(ExampleAttribute), false);
                _return = function.ReturnType;

                for (int i = 0; i < _parameters.Length; i++)
                {
                    _parameters[i] = param[i].ParameterType;
                    _parameterNames[i] = param[i].Name;
                }

                if (_return == typeof(void))
                {
                    _function = CreateFallback(function, target);
                    _return = typeof(Nothing);
                }
                else
                    _function = Create(function, target);
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets the names of the parameters.
            /// </summary>
            public String[] ParameterNames
            {
                get { return _parameterNames; }
            }

            /// <summary>
            /// Gets the return type.
            /// </summary>
            public Type Return
            {
                get { return _return; }
            }

            /// <summary>
            /// Gets the parameters.
            /// </summary>
            public Type[] Parameters
            {
                get { return _parameters; }
            }

            /// <summary>
            /// Gets a description of the function, given by a
            /// [Description] attribute on the method.
            /// </summary>
            public String Description
            {
                get
                {
                    var descriptions = Function.Method.GetCustomAttributes(typeof(DescriptionAttribute), false);

                    if (descriptions.Length == 0)
                        return NoDescriptionAttribute.Message;

                    var lines = new String[descriptions.Length];

                    for (int i = 0; i < descriptions.Length; i++)
                    {
                        var da = (DescriptionAttribute)descriptions[i];
                        lines[i] = da.Description;
                    }

                    return String.Join(Environment.NewLine, lines);
                }
            }

            /// <summary>
            /// Gets the concrete delegate.
            /// </summary>
            public Func<Dynamic[], Dynamic> Function
            {
                get { return _function; }
            }

            /// <summary>
            /// Gets an enumeration with example for the function.
            /// </summary>
            public IEnumerable<IExample> Examples
            {
                get
                {
                    foreach (ExampleAttribute example in _examples)
                    {
                        yield return new YExample
                        {
                            CodeSnippet = example.Example,
                            Description = example.Description
                        };
                    }
                }
            }

            #endregion

            #region Create Delegate

            Func<Dynamic[], Dynamic> CreateFallback(MethodInfo function, Object target)
            {
                return objs =>
                {
                    var args = objs.Select(m => m.Value).ToArray();
                    function.Invoke(target, args);
                    return new Dynamic();
                };
            }

            Func<Dynamic[], Dynamic> Create(MethodInfo method, Object target)
            {
                // First fetch the generic form
                var genericHelper = typeof(ReflectedFunctionItem).GetMethod("CreateHelper" + _parameters.Length, BindingFlags.Static | BindingFlags.NonPublic);

                //Create the type parameters
                var parameters = new Type[_parameters.Length + 2];
                parameters[0] = target.GetType();
                parameters[parameters.Length - 1] = method.ReturnType;

                for (int i = 0; i < _parameters.Length; i++)
                    parameters[i + 1] = _parameters[i];

                // Now supply the type arguments
                var constructedHelper = genericHelper.MakeGenericMethod(parameters);

                // Now call it. The null argument is because it's a static method.
                var ret = (Func<Object[], Object>)constructedHelper.Invoke(null, new Object[] { method, target });

                // Cast the result to the right kind of delegate and return it
                return args =>
                {
                    return new Dynamic(ret(args.Select(m => m.Value).ToArray()));
                };
            }

            static Func<Object[], Object> CreateHelper0<TTarget, TReturn>(MethodInfo method, Object target) where TTarget : class
            {
                // Convert the slow MethodInfo into a fast, strongly typed, open delegate
                Func<TTarget, TReturn> func = (Func<TTarget, TReturn>)Delegate.CreateDelegate(typeof(Func<TTarget, TReturn>), method);

                // Now create a more weakly typed delegate which will call the strongly typed one
                Func<Object[], Object> ret = param => func((TTarget)target);
                return ret;
            }

            static Func<Object[], Object> CreateHelper1<TTarget, TParam, TReturn>(MethodInfo method, Object target) where TTarget : class
            {
                // Convert the slow MethodInfo into a fast, strongly typed, open delegate
                Func<TTarget, TParam, TReturn> func = (Func<TTarget, TParam, TReturn>)Delegate.CreateDelegate(typeof(Func<TTarget, TParam, TReturn>), method);

                // Now create a more weakly typed delegate which will call the strongly typed one
                Func<Object[], Object> ret = param => func((TTarget)target, (TParam)param[0]);
                return ret;
            }

            static Func<Object[], Object> CreateHelper2<TTarget, TParam1, TParam2, TReturn>(MethodInfo method, Object target) where TTarget : class
            {
                // Convert the slow MethodInfo into a fast, strongly typed, open delegate
                Func<TTarget, TParam1, TParam2, TReturn> func = (Func<TTarget, TParam1, TParam2, TReturn>)Delegate.CreateDelegate(typeof(Func<TTarget, TParam1, TParam2, TReturn>), method);

                // Now create a more weakly typed delegate which will call the strongly typed one
                Func<Object[], Object> ret = param => func((TTarget)target, (TParam1)param[0], (TParam2)param[1]);
                return ret;
            }

            static Func<Object[], Object> CreateHelper3<TTarget, TParam1, TParam2, TParam3, TReturn>(MethodInfo method, Object target) where TTarget : class
            {
                // Convert the slow MethodInfo into a fast, strongly typed, open delegate
                Func<TTarget, TParam1, TParam2, TParam3, TReturn> func = (Func<TTarget, TParam1, TParam2, TParam3, TReturn>)Delegate.CreateDelegate(typeof(Func<TTarget, TParam1, TParam2, TParam3, TReturn>), method);

                // Now create a more weakly typed delegate which will call the strongly typed one
                Func<Object[], Object> ret = param => func((TTarget)target, (TParam1)param[0], (TParam2)param[1], (TParam3)param[2]);
                return ret;
            }

            static Func<Object[], Object> CreateHelper4<TTarget, TParam1, TParam2, TParam3, TParam4, TReturn>(MethodInfo method, Object target) where TTarget : class
            {
                // Convert the slow MethodInfo into a fast, strongly typed, open delegate
                Func<TTarget, TParam1, TParam2, TParam3, TParam4, TReturn> func = (Func<TTarget, TParam1, TParam2, TParam3, TParam4, TReturn>)Delegate.CreateDelegate(typeof(Func<TTarget, TParam1, TParam2, TParam3, TParam4, TReturn>), method);

                // Now create a more weakly typed delegate which will call the strongly typed one
                Func<Object[], Object> ret = param => func((TTarget)target, (TParam1)param[0], (TParam2)param[1], (TParam3)param[2], (TParam4)param[3]);
                return ret;
            }

            static Func<Object[], Object> CreateHelper5<TTarget, TParam1, TParam2, TParam3, TParam4, TParam5, TReturn>(MethodInfo method, Object target) where TTarget : class
            {
                // Convert the slow MethodInfo into a fast, strongly typed, open delegate
                Func<TTarget, TParam1, TParam2, TParam3, TParam4, TParam5, TReturn> func = (Func<TTarget, TParam1, TParam2, TParam3, TParam4, TParam5, TReturn>)Delegate.CreateDelegate(typeof(Func<TTarget, TParam1, TParam2, TParam3, TParam4, TParam5, TReturn>), method);

                // Now create a more weakly typed delegate which will call the strongly typed one
                Func<Object[], Object> ret = param => func((TTarget)target, (TParam1)param[0], (TParam2)param[1], (TParam3)param[2], (TParam4)param[3], (TParam5)param[4]);
                return ret;
            }

            static Func<Object[], Object> CreateHelper6<TTarget, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TReturn>(MethodInfo method, Object target) where TTarget : class
            {
                // Convert the slow MethodInfo into a fast, strongly typed, open delegate
                Func<TTarget, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TReturn> func = (Func<TTarget, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TReturn>)Delegate.CreateDelegate(typeof(Func<TTarget, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TReturn>), method);

                // Now create a more weakly typed delegate which will call the strongly typed one
                Func<Object[], Object> ret = param => func((TTarget)target, (TParam1)param[0], (TParam2)param[1], (TParam3)param[2], (TParam4)param[3], (TParam5)param[4], (TParam6)param[5]);
                return ret;
            }

            static Func<Object[], Object> CreateHelper7<TTarget, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TReturn>(MethodInfo method, Object target) where TTarget : class
            {
                // Convert the slow MethodInfo into a fast, strongly typed, open delegate
                Func<TTarget, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TReturn> func = (Func<TTarget, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TReturn>)Delegate.CreateDelegate(typeof(Func<TTarget, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TReturn>), method);

                // Now create a more weakly typed delegate which will call the strongly typed one
                Func<Object[], Object> ret = param => func((TTarget)target, (TParam1)param[0], (TParam2)param[1], (TParam3)param[2], (TParam4)param[3], (TParam5)param[4], (TParam6)param[5], (TParam7)param[6]);
                return ret;
            }

            static Func<Object[], Object> CreateHelper8<TTarget, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TReturn>(MethodInfo method, Object target) where TTarget : class
            {
                // Convert the slow MethodInfo into a fast, strongly typed, open delegate
                Func<TTarget, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TReturn> func = (Func<TTarget, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TReturn>)Delegate.CreateDelegate(typeof(Func<TTarget, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TReturn>), method);

                // Now create a more weakly typed delegate which will call the strongly typed one
                Func<Object[], Object> ret = param => func((TTarget)target, (TParam1)param[0], (TParam2)param[1], (TParam3)param[2], (TParam4)param[3], (TParam5)param[4], (TParam6)param[5], (TParam7)param[6], (TParam8)param[7]);
                return ret;
            }

            static Func<Object[], Object> CreateHelper9<TTarget, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TReturn>(MethodInfo method, object target) where TTarget : class
            {
                // Convert the slow MethodInfo into a fast, strongly typed, open delegate
                Func<TTarget, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TReturn> func = (Func<TTarget, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TReturn>)Delegate.CreateDelegate(typeof(Func<TTarget, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TReturn>), method);

                // Now create a more weakly typed delegate which will call the strongly typed one
                Func<Object[], Object> ret = param => func((TTarget)target, (TParam1)param[0], (TParam2)param[1], (TParam3)param[2], (TParam4)param[3], (TParam5)param[4], (TParam6)param[5], (TParam7)param[6], (TParam8)param[7], (TParam9)param[8]);
                return ret;
            }

            static Func<Object[], Object> CreateHelper10<TTarget, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TReturn>(MethodInfo method, object target) where TTarget : class
            {
                // Convert the slow MethodInfo into a fast, strongly typed, open delegate
                Func<TTarget, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TReturn> func = (Func<TTarget, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TReturn>)Delegate.CreateDelegate(typeof(Func<TTarget, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TReturn>), method);

                // Now create a more weakly typed delegate which will call the strongly typed one
                Func<Object[], Object> ret = param => func((TTarget)target, (TParam1)param[0], (TParam2)param[1], (TParam3)param[2], (TParam4)param[3], (TParam5)param[4], (TParam6)param[5], (TParam7)param[6], (TParam8)param[7], (TParam9)param[8], (TParam10)param[9]);
                return ret;
            }

            delegate TResult Func<T1, T2, T3, T4, T5, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
            delegate TResult Func<T1, T2, T3, T4, T5, T6, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
            delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
            delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);
            delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);
            delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);
            delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);
            delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);

            #endregion
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
