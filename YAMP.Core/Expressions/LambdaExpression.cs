using System;
using System.Collections.Generic;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Parser;
using YAMP.Statements;

namespace YAMP.Expressions
{
    /// <summary>
    /// Represents a lambda expression.
    /// </summary>
    class LambdaExpression : Expression
    {
        #region Members

        FunctionStatement _function;

        #endregion

        #region ctor

        public LambdaExpression(Token token, ListExpression arguments, Statement body)
            : base(token)
        {
            _function = new FunctionStatement(token);
            _function.Arguments = arguments ?? new ListExpression(token);
            _function.Add(body ?? Statement.Empty);
        }

        #endregion

        #region Methods

        public override Dynamic Evaluate(RunContext ctx)
        {
            return _function.Evaluate(ctx);
        }

        #endregion

        #region String Representation

        /// <summary>
        /// Transforms the expression into code.
        /// </summary>
        /// <returns>The code that represents the statement.</returns>
        public override string ToCode()
        {
            return _function.Arguments.ToCode() + " => " + _function.BodyToCode();
        }

        #endregion
    }
}
