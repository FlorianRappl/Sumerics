using System;
using YAMP.Core;

namespace YAMP.Statements
{
    /// <summary>
    /// Represents a statement.
    /// </summary>
    abstract class Statement
    {
        #region Members

        static readonly Statement _empty = new EmptyStatement();

        #endregion

        #region Properties

        /// <summary>
        /// Gets an empty statement.
        /// </summary>
        public static Statement Empty
        {
            get { return _empty; }
        }

        /// <summary>
        /// Gets if the statement should be assigned.
        /// </summary>
        public virtual bool IsAssigned
        {
            get { return true; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Evaluates the given query in the given context.
        /// </summary>
        /// <param name="ctx">The context of the evaluation.</param>
        /// <returns>The result of the query.</returns>
        public abstract Dynamic Evaluate(RunContext ctx);

        #endregion

        #region String Representation

        /// <summary>
        /// Transforms the statement into code.
        /// </summary>
        /// <returns>The code that represents the statement.</returns>
        public virtual string ToCode()
        {
            return string.Empty;
        }

        #endregion
    }
}
