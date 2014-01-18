using System;
using YAMP.Parser;
using YAMP.Core;
using YAMP.Statements;

namespace YAMP.Expressions
{
    /// <summary>
    /// Represents an expression.
    /// </summary>
    abstract class Expression : Statement
    {
        #region Members

        protected Token _token;

        #endregion

        #region ctor

        public Expression(Token token)
        {
            _token = token;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the assigned token.
        /// </summary>
        public Token Token
        {
            get { return _token; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Evaluates the given query in the given context.
        /// </summary>
        /// <param name="ctx">The context of the evaluation.</param>
        /// <returns>The result of the query.</returns>
        public override Dynamic Evaluate(RunContext ctx)
        {
            return new Dynamic(Token.Value);
        }

        #endregion
    }
}
