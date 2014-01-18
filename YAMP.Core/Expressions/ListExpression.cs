using System;
using System.Collections;
using System.Collections.Generic;
using YAMP.Parser;
using YAMP.Core;

namespace YAMP.Expressions
{
    /// <summary>
    /// Represents a list of expressions.
    /// </summary>
    sealed class ListExpression : Expression, IEnumerable<Expression>
    {
        #region Members

        List<Expression> _expressions;

        #endregion

        #region ctor

        public ListExpression(Token starter)
            : base(starter)
        {
            _expressions = new List<Expression>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets if the statement should be assigned.
        /// </summary>
        public override Boolean IsAssigned
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the length of the list.
        /// </summary>
        public Int32 Length
        {
            get { return _expressions.Count; }
        }

        /// <summary>
        /// Gets a specific expression from the list.
        /// </summary>
        /// <param name="index">The 0-based index.</param>
        /// <returns>The expression.</returns>
        public Expression this[Int32 index]
        {
            get { return _expressions[index]; }
        }

        #endregion

        #region Methods

        public override Dynamic Evaluate(RunContext ctx)
        {
            var o = new Dynamic[_expressions.Count];

            for (int i = 0; i < _expressions.Count; i++)
                o[i] = _expressions[i].Evaluate(ctx);

            return new Dynamic(o);
        }

        public void Add(Expression expression)
        {
            _expressions.Add(expression);
        }

        public IEnumerator<Expression> GetEnumerator()
        {
            return _expressions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_expressions).GetEnumerator();
        }

        #endregion

        #region String Representation

        /// <summary>
        /// Transforms the expression into code.
        /// </summary>
        /// <returns>The code that represents the statement.</returns>
        public override String ToCode()
        {
            var entries = new String[Length];

            for (var i = 0; i < Length; i++)
                entries[i] = _expressions[i].ToCode();

            return "(" + String.Join(", ", entries) + ")";
        }

        #endregion
    }
}
