using System;
using YAMP.Core;

namespace YAMP.Statements
{
    /// <summary>
    /// Represents the empty statement.
    /// </summary>
    class EmptyStatement : Statement
    {
        #region Methods

        /// <summary>
        /// Evaluates the given query in the given context.
        /// </summary>
        /// <param name="ctx">The context of the evaluation.</param>
        /// <returns>The result of the query.</returns>
        public override Dynamic Evaluate(RunContext ctx)
        {
            return null;
        }

        #endregion
    }
}
