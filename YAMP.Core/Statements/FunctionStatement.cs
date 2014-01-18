using System;
using System.Collections.Generic;
using YAMP.Expressions;
using YAMP.Parser;
using YAMP.Core;
using YAMP.Attributes;

namespace YAMP.Statements
{
    /// <summary>
    /// Represents a function.
    /// </summary>
    class FunctionStatement : StatementList
    {
        #region Members

        Token _name;
        ListExpression _args;
        Token _start;
        Type[] _parameters;
        String[] _parameterNames;

        #endregion

        #region ctor

        public FunctionStatement(Token start)
        {
            _start = start;
        }

        #endregion

        #region Properties

        public bool IsAnonymous
        {
            get { return _name == null; }
        }

        public Token Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Type[] Parameters
        {
            get { return _parameters; }
        }

        public String[] ParameterNames
        {
            get { return _parameterNames; }
        }

        public ListExpression Arguments
        {
            get { return _args; }
            set 
            { 
                _args = value;
                _parameters = new Type[_args.Length];
                _parameterNames = new String[_args.Length];

                for (int i = 0; i < _parameters.Length; i++)
                {
                    _parameters[i] = typeof(Object);
                    _parameterNames[i] = _args[i].Token.ValueAsString;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new representation for the given function data.
        /// </summary>
        /// <param name="context">The context to use.</param>
        /// <returns>The function item.</returns>
        public IFunctionItem Create(Scope context)
        {
            return new Representation(this, context);
        }

        /// <summary>
        /// Evaluates the given query in the given context.
        /// </summary>
        /// <param name="ctx">The context of the evaluation.</param>
        /// <returns>The result of the query.</returns>
        public override Dynamic Evaluate(RunContext ctx)
        {
            var f = IsAnonymous ? CustomFunction.Anonymous : ctx.SetFunction(_name.ValueAsString);
            f.AddOverload(new Representation(this, ctx.Current));
            return IsAnonymous ? new Dynamic(f) : null;
        }

        #endregion

        #region String Representation

        /// <summary>
        /// Transforms the statement into code.
        /// </summary>
        /// <returns>The code that represents the statement.</returns>
        public override string ToCode()
        {
            return string.Format("function {0}{1} {{{2}{3}{2}}}", 
                (IsAnonymous ? string.Empty : _name.ValueAsString), _args.ToCode(), Environment.NewLine, base.ToCode());
        }

        /// <summary>
        /// Transforms the body of the function into code.
        /// </summary>
        /// <returns>The code that represents the body of the function.</returns>
        public string BodyToCode()
        {
            return base.ToCode();
        }

        #endregion

        #region IFunctionItem implementation

        sealed class Representation : IFunctionItem
        {
            FunctionStatement _parent;
            Scope _context;

            public Representation(FunctionStatement parent, Scope context)
            {
                _parent = parent;
                _context = context;
            }

            public Type[] Parameters
            {
                get { return _parent._parameters; }
            }

            public String[] ParameterNames
            {
                get { return _parent._parameterNames; }
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
                get { return Run; }
            }

            public IEnumerable<IExample> Examples
            {
                get { yield break; }
            }

            Dynamic Run(Dynamic[] args)
            {
                var index = 0;
                Scope local = new Scope();
                local.Parent = _context;
                //TODO
                //ctx.CreateScope(local);

                for (var i = 0; i < _parent._parameterNames.Length; i++)
                    local.Set(_parent._parameterNames[i], args[i]);

                while (index < _parent._statements.Count)
                {
                    //_parent._statements[index++].Evaluate(ctx);

                    //if (ctx.ShouldStop)
                    //    break;
                }

                //ctx.ReleaseScope();
                //return ctx.ClearReturnMarker();
                return null;
            }
        }

        #endregion
    }
}
