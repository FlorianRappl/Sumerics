using System;
using YAMP.Parser;
using YAMP.Core;

namespace YAMP.Statements
{
    /// <summary>
    /// Represents the for loop.
    /// </summary>
    class ForLoopStatement : StatementList
    {
        #region Members

        Statement _init;
        Statement _condition;
        Statement _step;
        Token _start;

        #endregion

        #region ctor

        public ForLoopStatement(Token start)
        {
            _start = start;
            _step = Statement.Empty;
            _init = Statement.Empty;
            _condition = Statement.Empty;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the initializer of for loop.
        /// </summary>
        public Statement Initializer
        {
            get { return _init; }
            set { _init = value; }
        }

        /// <summary>
        /// Gets or sets the condition of for loop.
        /// </summary>
        public Statement Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }

        /// <summary>
        /// Gets or sets the step of for loop.
        /// </summary>
        public Statement Step
        {
            get { return _step; }
            set { _step = value; }
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
            _init.Evaluate(ctx);

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

                _step.Evaluate(ctx);
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
            return string.Format("for ({2}; {3}; {4}) {{{0}{1}{0}}}",
                Environment.NewLine, base.ToCode(), 
                _init.ToCode(),
                _condition.ToCode(), 
                _step.ToCode());
        }

        #endregion
    }
}
