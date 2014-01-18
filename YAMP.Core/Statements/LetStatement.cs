using System;
using YAMP.Expressions;
using YAMP.Parser;
using YAMP.Core;

namespace YAMP.Statements
{
    /// <summary>
    /// Represents a let instruction.
    /// </summary>
    class LetStatement : Statement
    {
        #region Members

        Token _start;
        Token _name;
        Expression _assignment;

        #endregion

        #region ctor

        public LetStatement(Token start)
        {
            _start = start;
        }

        #endregion

        #region Properties

        public Token Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public bool IsValueAssigned
        {
            get { return _assignment != null; }
        }

        public Expression Assignment
        {
            get { return _assignment; }
            set { _assignment = value; }
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
            var value = IsValueAssigned ? _assignment.Evaluate(ctx) : new Dynamic(0);
            ctx.Current.Set(_name.ValueAsString, value);
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
            var assign = IsValueAssigned ? " = " + _assignment.ToCode() : string.Empty;
            return string.Format("let {0}{1}", _name.ValueAsString, assign);
        }

        #endregion
    }
}
