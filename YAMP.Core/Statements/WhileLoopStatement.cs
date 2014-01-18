using System;
using YAMP.Expressions;
using YAMP.Parser;
using YAMP.Core;

namespace YAMP.Statements
{
    /// <summary>
    /// Represents the while loop.
    /// </summary>
    class WhileLoopStatement : StatementList
    {
        #region Members

        Expression _condition;
        Token _start;

        #endregion

        #region ctor

        public WhileLoopStatement(Token start)
        {
            _start = start;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the condition of the do-while loop.
        /// </summary>
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
            int index;
            var scope = new Scope();
            ctx.CreateBreakableScope(scope);

            while (_condition.Evaluate(ctx))
            {
                index = 0;

                while (index < _statements.Count)
                {
                    _statements[index++].Evaluate(ctx);

                    if (ctx.ShouldStop)
                        break;
                }

                if (ctx.ShouldStop)
                    break;
            }

            ctx.ReleaseBreakableScope();
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
            var c = _condition != null ? _condition.ToCode() : string.Empty;
            return string.Format("while ({2}) {{{0}{1}{0}}}",
                Environment.NewLine, base.ToCode(), c);
        }

        #endregion
    }
}
