using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Expressions;
using YAMP.Parser;

namespace YAMP.Statements
{
    /// <summary>
    /// Represents the class statement.
    /// </summary>
    class ClassStatement : Statement
    {
        /*
            class point
            {
                let x;
                let y;
                
                function point() { 
                    x = 0; 
                    y = 0; 
                } 
                
                function point(xl, yl) { 
                    x = xl; 
                    y = yl; 
                } 
                
                function abs() { 
                    return sqrt(x * x + y * y);
                } 
            }
         */

        #region Members

        Token _name;
        Token _start;
        readonly List<LetStatement> _init;
        readonly List<FunctionStatement> _functions;
        readonly List<FunctionStatement> _constructors;
        //readonly List<ClassStatement> _classes;
        readonly StringCollection _globals;

        #endregion

        #region ctor

        public ClassStatement(Token start)
        {
            _start = start;
            _init = new List<LetStatement>();
            _functions = new List<FunctionStatement>();
            _constructors = new List<FunctionStatement>();
            //_classes = new List<ClassStatement>();
            _globals = new StringCollection();
        }

        #endregion

        #region Properties

        public Token Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion

        #region Methods

        //public bool TryAddClass(ClassStatement cls)
        //{
        //    var name = cls.Name.ValueAsString;

        //    if (!_globals.Contains(name))
        //    {
        //        _classes.Add(cls);
        //        _globals.Add(name);
        //        return true;
        //    }

        //    return false;
        //}

        public bool TryAddFunction(FunctionStatement func)
        {
            var name = func.Name.ValueAsString;
            var isoverload = false;

            if (name == _name.ValueAsString)
            {
                for (int i = 0; i < _constructors.Count; i++)
                {
                    if (_constructors[i].Arguments.Length == func.Arguments.Length)
                        return false;
                }

                _constructors.Add(func);
                return true;
            }

            for (int i = 0; i < _functions.Count; i++)
            {
                if (_functions[i].Name.ValueAsString == name)
                {
                    isoverload = true;

                    if (_functions[i].Arguments.Length == func.Arguments.Length)
                        return false;
                }
            }

            if (isoverload || !_globals.Contains(name))
            {
                _functions.Add(func);
                _globals.Add(name);
                return true;
            }

            return false;
        }

        public bool TryAddInitializer(LetStatement let)
        {
            var name = let.Name.ValueAsString;

            if (!_globals.Contains(name))
            {
                _init.Add(let);
                _globals.Add(name);
                return true;
            }

            return false;
        }

        public override Dynamic Evaluate(RunContext ctx)
        {
            var f = ctx.SetFunction(_name.ValueAsString);

            if (_constructors.Count > 0)
            {
                for (int i = 0; i < _constructors.Count; i++)
                    f.AddOverload(new ClassConstructor(this, _constructors[i]));
            }
            else
                f.AddOverload(new ClassConstructor(this));

            return null;
        }

        #endregion

        #region Constructors

        sealed class ClassConstructor : IFunctionItem
        {
            Type[] parameters;
            String[] parameterNames;
            Func<Dynamic[], Dynamic> function;

            public ClassConstructor(ClassStatement that, FunctionStatement statements)
            {
                parameterNames = statements.ParameterNames;
                parameters = statements.Parameters;

                function = args =>
                {
                    var global = new Scope();
                    //TODO
                    //ctx.CreateTopScope(global);

                    //for (int i = 0; i < that._init.Count; i++)
                    //    that._init[i].Evaluate(ctx);

                    //var local = new Scope();
                    //ctx.CreateScope(local);

                    //int index = 0;

                    //for (int i = 0; i < parameterNames.Length; i++)
                    //    local.Set(parameterNames[i], args[i]);

                    //while (index < statements.Count)
                    //{
                    //    statements[index++].Evaluate(ctx);

                    //    if (ctx.ShouldStop)
                    //        break;
                    //}

                    //ctx.ReleaseScope();
                    //ctx.ClearReturnMarker();
                    //ctx.ReleaseScope();
                    return new Dynamic(new CustomObject(that, global));
                };
            }

            public ClassConstructor(ClassStatement that)
            {
                parameters = new Type[0];
                parameterNames = new String[0];

                function = args =>
                {
                    var global = new Scope();
                    //TODO
                    //ctx.CreateTopScope(global);

                    //for (int i = 0; i < that._init.Count; i++)
                    //    that._init[i].Evaluate(ctx);

                    //ctx.ReleaseScope();
                    return new Dynamic(new CustomObject(that, global));
                };
            }

            public Type[] Parameters
            {
                get { return parameters; }
            }

            public String[] ParameterNames
            {
                get { return parameterNames; }
            }

            public Type Return
            {
                get { return typeof(Object); }
            }

            public String Description
            {
                get { return NoDescriptionAttribute.Message; }
            }

            public Func<Dynamic[], Dynamic> Function
            {
                get { return function; }
            }

            public IEnumerable<IExample> Examples
            {
                get { yield break; }
            }
        }

        #endregion

        #region Object

        class CustomObject : IObject
        {
            Scope globals;
            ClassStatement defs;

            public CustomObject(ClassStatement that, Scope scope)
            {
                globals = scope;
                defs = that;
            }

            public CustomFunction GetMethod(string name)
            {
                CustomFunction f = new CustomFunction(name);

                for (int i = 0; i < defs._functions.Count; i++)
                {
                    if (defs._functions[i].Name.ValueAsString == name)
                        f.AddOverload(defs._functions[i].Create(globals));
                }

                return f;
            }

            public bool HasMethod(string name)
            {
                for (int i = 0; i < defs._functions.Count; i++)
                {
                    if (defs._functions[i].Name.ValueAsString == name)
                        return true;
                }

                return false;
            }

            public bool HasProperty(string name)
            {
                for (int i = 0; i < defs._init.Count; i++)
                {
                    if (defs._init[i].Name.ValueAsString == name)
                        return true;
                }

                return false;
            }

            public string[] Methods
            {
                get 
                {
                    var list = new string[defs._functions.Count];

                    for (int i = 0; i < defs._functions.Count; i++)
                        list[i] = defs._functions[i].Name.ValueAsString;

                    return list;
                }
            }

            public string[] Properties
            {
                get
                {
                    var list = new string[defs._init.Count];

                    for (int i = 0; i < defs._init.Count; i++)
                        list[i] = defs._init[i].Name.ValueAsString;

                    return list;
                }
            }

            public bool TryReadProperty(string name, out Dynamic value)
            {
                value = globals.Get(name);
                return true;
            }

            public bool TryWriteProperty(string name, Dynamic value)
            {
                globals.Set(name, value);
                return true;
            }

            public override string ToString()
            {
                return "{ " + globals.ToString() + " }";
            }
        }

        #endregion
    }
}
