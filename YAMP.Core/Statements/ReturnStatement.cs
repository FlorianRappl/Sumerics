using System;
using YAMP.Expressions;
using YAMP.Parser;
using YAMP.Core;

namespace YAMP.Statements
{
    /// <summary>
    /// Represents the return statement.
    /// </summary>
    class ReturnStatement : Statement
    {
        #region Members

        Token _start;
        Statement _payload;

        #endregion

        #region ctor

        public ReturnStatement(Token start)
        {
            _start = start;
            _payload = Statement.Empty;
        }

        #endregion

        #region Properties

        public Statement Payload
        {
            get { return _payload; }
            set { _payload = value; }
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
            ctx.Return(_payload.Evaluate(ctx));
            return null;
        }

        #endregion

        #region String Representation

        /// <summary>
        /// Transforms the statement into code.
        /// </summary>
        /// <returns>The code that represents the statement.</returns>
        public override string ToCode()
        {
            return string.Format("return {0};", _payload.ToCode());
        }

        #endregion
    }
}
