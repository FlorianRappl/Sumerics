using System;
using YAMP.Expressions;
using YAMP.Parser;
using YAMP.Core;

namespace YAMP.Statements
{
    /// <summary>
    /// Represents the conditional statement.
    /// </summary>
    class ConditionalStatement : Statement
    {
        #region Members

        Statement _body;
        Statement _otherwise;
        Expression _condition;
        Token _start;

        #endregion

        #region ctor

        public ConditionalStatement(Token start)
        {
            _start = start;
            _body = Statement.Empty;
            _otherwise = Statement.Empty;
        }

        #endregion

        #region Properties

        public Statement Body
        {
            get { return _body; }
            set { _body = value; }
        }

        public Statement Otherwise
        {
            get { return _otherwise; }
            set { _otherwise = value; }
        }

        public Expression Condition
        {
            get { return _condition; }
            set { _condition = value; }
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
            if (_condition.Evaluate(ctx))
                _body.Evaluate(ctx);
            else
                _otherwise.Evaluate(ctx);

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
            return string.Format("if ({0}) {{{2}{1}{2}}}{3}",
                _condition.ToCode(), _body.ToCode(), Environment.NewLine, 
                _otherwise != null ? string.Format(" else {{{0}{1}{0}}}", Environment.NewLine, _otherwise.ToCode()) : string.Empty);
        }

        #endregion
    }
}
